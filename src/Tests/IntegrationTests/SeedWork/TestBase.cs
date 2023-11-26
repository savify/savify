using System.Data;
using App.API;
using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Tests.IntegrationTests;
using App.BuildingBlocks.Tests.IntegrationTests.Probing;
using App.Database.Scripts.Clear;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.UserAccess.Application.Contracts;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected IUserAccessModule UserAccessModule { get; private set; }

    protected INotificationsModule NotificationsModule { get; private set; }

    protected string ConnectionString { get; private set; }

    [OneTimeSetUp]
    public void Init()
    {
        const string connectionStringEnvironmentVariable = "ASPNETCORE_INTEGRATION_TESTS_CONNECTION_STRING";
        ConnectionString = EnvironmentVariablesProvider.GetVariable(connectionStringEnvironmentVariable);

        if (ConnectionString == null)
        {
            throw new ApplicationException(
                $"Define connection string to integration tests database using environment variable: {connectionStringEnvironmentVariable}");
        }

        WebApplicationFactory = new CustomWebApplicationFactory<Program>();
        CompositionRoot.SetServiceProvider(WebApplicationFactory.Services);

        using var scope = WebApplicationFactory.Services.CreateScope();
        UserAccessModule = scope.ServiceProvider.GetRequiredService<IUserAccessModule>();
        NotificationsModule = scope.ServiceProvider.GetRequiredService<INotificationsModule>();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        WebApplicationFactory.Dispose();
    }

    [SetUp]
    public async Task BeforeEachTest()
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);
        await ClearDatabase(sqlConnection);
    }

    protected static async Task AssertEventually(IProbe probe, int timeout)
    {
        await new Poller(timeout).CheckAsync(probe);
    }

    protected static void AssertBrokenRule<TRule>(AsyncTestDelegate testDelegate) where TRule : class, IBusinessRule
    {
        var message = $"Expected {typeof(TRule).Name} broken rule";
        var businessRuleValidationException =
            Assert.CatchAsync<BusinessRuleValidationException>(testDelegate, message);

        if (businessRuleValidationException != null)
        {
            Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
        }
    }

    private static async Task ClearDatabase(IDbConnection connection)
    {
        var sql = await ClearDatabaseCommandProvider.GetAsync();

        await connection.ExecuteScalarAsync(sql);
    }
}
