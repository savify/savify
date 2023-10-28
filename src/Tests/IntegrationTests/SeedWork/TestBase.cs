using System.Data;
using App.API;
using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Tests.IntegrationTests;
using App.BuildingBlocks.Tests.IntegrationTests.Probing;
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
        // TODO: move to some external class/sql file
        const string sql = "DELETE FROM user_access.outbox_messages; " +
                           "DELETE FROM user_access.roles_permissions; " +
                           "DELETE FROM user_access.users; " +
                           "DELETE FROM user_access.user_roles; " +
                           "DELETE FROM user_access.permissions; " +
                           "DELETE FROM user_access.inbox_messages; " +
                           "DELETE FROM user_access.internal_commands; " +
                           "DELETE FROM user_access.user_registrations; " +
                           "DELETE FROM user_access.password_reset_requests; " +
                           "DELETE FROM user_access.refresh_tokens; " +
                           "DELETE FROM notifications.inbox_messages; " +
                           "DELETE FROM notifications.internal_commands; " +
                           "DELETE FROM notifications.user_notification_settings; " +
                           "DELETE FROM wallets.internal_commands; " +
                           "DELETE FROM wallets.outbox_messages; " +
                           "DELETE FROM wallets.cash_wallets; " +
                           "DELETE FROM wallets.debit_wallets; " +
                           "DELETE FROM wallets.credit_wallets; " +
                           "DELETE FROM wallets.wallet_view_metadata; " +
                           "DELETE FROM wallets.inbox_messages; ";

        await connection.ExecuteScalarAsync(sql);
    }
}
