using System.Data;
using App.API;
using App.BuildingBlocks.Tests.IntegrationTests;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Infrastructure.Configuration;
using App.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
using Dapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.Modules.UserAccess.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected IUserAccessModule UserAccessModule { get; private set; }

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
        UserAccessCompositionRoot.SetServiceProvider(WebApplicationFactory.Services);
    }

    [SetUp]
    public async Task BeforeEachTest()
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);
        await ClearDatabase(sqlConnection);
    }

    protected async Task<List<OutboxMessageDto>> GetOutboxMessages()
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        var messages = await OutboxMessagesAccessor.GetOutboxMessages(connection);

        return messages;
    }

    protected async Task<T> GetLastOutboxMessage<T>() where T : class, INotification
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        var messages = await OutboxMessagesAccessor.GetOutboxMessages(connection);

        return OutboxMessagesAccessor.Deserialize<T>(messages.Last());
    }

    private static async Task ClearDatabase(IDbConnection connection)
    {
        const string sql = "DELETE FROM user_access.internal_commands; " +
                           "DELETE FROM user_access.outbox_messages; " +
                           "DELETE FROM user_access.roles_permissions; " +
                           "DELETE FROM user_access.users; " +
                           "DELETE FROM user_access.user_roles; " +
                           "DELETE FROM user_access.permissions; " +
                           "DELETE FROM user_access.inbox_messages; " +
                           "DELETE FROM user_access.user_registrations; " +
                           "DELETE FROM user_access.password_reset_requests; " +
                           "DELETE FROM user_access.refresh_tokens; ";

        await connection.ExecuteScalarAsync(sql);
    }
}
