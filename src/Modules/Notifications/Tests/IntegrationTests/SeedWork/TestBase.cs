using System.Data;
using App.API;
using App.BuildingBlocks.Tests.IntegrationTests;
using App.Database.Scripts.Clear;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Infrastructure.Configuration;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;

namespace App.Modules.Notifications.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected INotificationsModule NotificationsModule { get; private set; }

    protected string ConnectionString { get; private set; }

    protected IEmailSender EmailSender { get; private set; }

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

        EmailSender = Substitute.For<IEmailSender>();
        WebApplicationFactory = new CustomWebApplicationFactory<Program>(EmailSender);

        using var scope = WebApplicationFactory.Services.CreateScope();
        NotificationsModule = scope.ServiceProvider.GetRequiredService<INotificationsModule>();
        NotificationsCompositionRoot.SetServiceProvider(WebApplicationFactory.Services);
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

    private static async Task ClearDatabase(IDbConnection connection)
    {
        var sql = await ClearDatabaseCommandProvider.GetAsync();

        await connection.ExecuteScalarAsync(sql);
    }
}
