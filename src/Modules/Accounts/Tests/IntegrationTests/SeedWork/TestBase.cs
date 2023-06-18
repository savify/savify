using System.Data;
using App.API;
using App.BuildingBlocks.Tests.IntegrationTests;
using App.Modules.Accounts.Application.Contracts;
using App.Modules.Accounts.Infrastructure.Configuration;
using App.Modules.Accounts.Infrastructure.Configuration.Processing.Outbox;
using Dapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.Modules.Accounts.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }
    
    protected IAccountsModule AccountsModule { get; private set; }
    
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
        AccountsModule = scope.ServiceProvider.GetRequiredService<IAccountsModule>();
        AccountsCompositionRoot.SetServiceProvider(WebApplicationFactory.Services);
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
        const string sql = "DELETE FROM accounts.internal_commands; " +
                           "DELETE FROM accounts.outbox_messages; " +
                           "DELETE FROM accounts.inbox_messages; " +
                           "DELETE FROM accounts.cash_accounts; " +
                           "DELETE FROM accounts.credit_accounts;";

        await connection.ExecuteScalarAsync(sql);
    }
}
