using App.BuildingBlocks.Tests.IntegrationTests.Probing;
using App.Modules.Categories.Application.Categories.GetCategories;
using App.Modules.Categories.Application.CategoriesSynchronisationProcessing.SynchroniseCategories;
using Dapper;
using Npgsql;

using FinanceTrackingDatabaseConfiguration = App.Modules.FinanceTracking.Application.Configuration.Data.DatabaseConfiguration;

namespace App.IntegrationTests.CategoriesSynchronisation;

[TestFixture]
public class CategoriesSynchronisationTests : TestBase
{
    [Test]
    public async Task CreatesCategoryRepresentation_InFinanceTrackingModule_WhenCategoriesSynchronisationWasExecuted_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchCategoriesSuccessfulResponse();

        await CategoriesModule.ExecuteCommandAsync(new SynchroniseCategoriesCommand());

        var categories = await CategoriesModule.ExecuteQueryAsync(new GetCategoriesQuery());
        var expectedCategories = categories.Select(c => new CategoryIds(c.Id, c.ExternalId)).ToList();

        await AssertEventually(
            new GetCreatedCategoriesFromFinanceTrackingProbe(expectedCategories, ConnectionString));
    }

    private class GetCreatedCategoriesFromFinanceTrackingProbe(
        List<CategoryIds> expectedCategories,
        string connectionString)
        : IProbe
    {
        private IList<CategoryIds> _categories = new List<CategoryIds>();

        public bool IsSatisfied()
        {
            if (_categories.Count == expectedCategories.Count)
            {
                return _categories.All(c => expectedCategories.Any(ec => ec.ExternalId == c.ExternalId));
            }

            return false;
        }

        public async Task SampleAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);

                _categories = (await connection.QueryAsync<CategoryIds>(
                    $"SELECT id, external_id AS externalId FROM {FinanceTrackingDatabaseConfiguration.Schema.Name}.categories")).ToList();
            }
            catch
            {
                // ignored
            }
        }

        public string DescribeFailureTo() => "Categories representations were not created in FinanceTracking module";
    }

    private record CategoryIds(Guid Id, string ExternalId);
}
