using System.Data;
using App.API;
using App.BuildingBlocks.Tests.IntegrationTests;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Infrastructure.Configuration;
using App.Modules.Wallets.Infrastructure.Configuration.Processing.Outbox;
using App.Modules.Wallets.IntegrationTests.SeedData;
using Dapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WireMock.Server;

namespace App.Modules.Wallets.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected SaltEdgeHttpClientMocker SaltEdgeHttpClientMocker { get; private set; }

    protected IWalletsModule WalletsModule { get; private set; }

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
        WalletsModule = scope.ServiceProvider.GetRequiredService<IWalletsModule>();
        WalletsCompositionRoot.SetServiceProvider(WebApplicationFactory.Services);

        SaltEdgeHttpClientMocker = new SaltEdgeHttpClientMocker(WireMockServer.StartWithAdminInterface(port: 1080, ssl: false));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        SaltEdgeHttpClientMocker.StopWireMockServer();
        WebApplicationFactory.Dispose();
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
        const string sql = "DELETE FROM wallets.internal_commands; " +
                           "DELETE FROM wallets.outbox_messages; " +
                           "DELETE FROM wallets.inbox_messages; " +
                           "DELETE FROM wallets.cash_wallets; " +
                           "DELETE FROM wallets.credit_wallets; " +
                           "DELETE FROM wallets.debit_wallets;" +
                           "DELETE FROM wallets.wallet_view_metadata;" +
                           "DELETE FROM wallets.bank_accounts;" +
                           "DELETE FROM wallets.bank_connection_processes;" +
                           "DELETE FROM wallets.bank_connections;" +
                           "DELETE FROM wallets.salt_edge_connections;" +
                           "DELETE FROM wallets.salt_edge_customers;";

        await connection.ExecuteScalarAsync(sql);
    }
}
