using App.Modules.Categories.Application.Categories.GetCategories;
using App.Modules.Categories.Application.CategoriesSynchronisationProcessing.SynchroniseCategories;
using App.Modules.Categories.Domain.Categories;
using App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;
using App.Modules.Categories.IntegrationTests.SeedWork;

namespace App.Modules.Categories.IntegrationTests.CategoriesSynchronisationProcessing;

[TestFixture]
public class SynchroniseCategoriesCommandTests : TestBase
{
    [Test]
    public async Task SynchroniseCategoriesCommand_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchCategoriesSuccessfulResponse();

        var result = await CategoriesModule.ExecuteCommandAsync(new SynchroniseCategoriesCommand());

        var categories = (await CategoriesModule.ExecuteQueryAsync(new GetCategoriesQuery())).ToList();

        Assert.That(result.Status, Is.EqualTo(CategoriesSynchronisationProcessStatus.Finished.Value));
        Assert.That(categories.Count, Is.EqualTo(6));

        Assert.That(categories.Any(c => c.ExternalId == "paycheck" && c.Type == CategoryType.Income.Value));
        Assert.That(categories.Any(c => c.ExternalId == "bonus" && c.Type == CategoryType.Income.Value));
        Assert.That(categories.Any(c => c.ExternalId == "shopping" && c.Type == CategoryType.Expense.Value));
        Assert.That(categories.Any(c => c.ExternalId == "home" && c.Type == CategoryType.Expense.Value));
        Assert.That(categories.Any(c => c.ExternalId == "clothing" &&
                                        c.Type == CategoryType.Expense.Value &&
                                        c.ParentId == categories.Single(p => p.ExternalId == "shopping").Id));

        Assert.That(categories.Any(c => c.ExternalId == "rent" &&
                                        c.Type == CategoryType.Expense.Value &&
                                        c.ParentId == categories.Single(p => p.ExternalId == "home").Id));
    }

    [Test]
    public async Task SynchroniseCategoriesCommand_WhenErrorAtProviderOccurred_WillReturnFailedStatus_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchCategoriesErrorResponse();

        var result = await CategoriesModule.ExecuteCommandAsync(new SynchroniseCategoriesCommand());

        var categories = (await CategoriesModule.ExecuteQueryAsync(new GetCategoriesQuery())).ToList();

        Assert.That(result.Status, Is.EqualTo(CategoriesSynchronisationProcessStatus.Failed.Value));
        Assert.That(categories.Count, Is.EqualTo(0));
    }
}
