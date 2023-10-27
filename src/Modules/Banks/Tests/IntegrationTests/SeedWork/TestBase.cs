using System.Data;
using App.API;
using App.BuildingBlocks.Tests.IntegrationTests;
using App.Modules.Banks.Application.Contracts;
using App.Modules.Banks.Infrastructure.Configuration;
using App.Modules.Banks.Infrastructure.Configuration.Processing.Outbox;
using App.Modules.Banks.IntegrationTests.SeedData;
using Dapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WireMock.Server;

namespace App.Modules.Banks.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected SaltEdgeHttpClientMocker SaltEdgeHttpClientMocker { get; private set; }

    protected IBanksModule BanksModule { get; private set; }

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
        BanksModule = scope.ServiceProvider.GetRequiredService<IBanksModule>();
        BanksCompositionRoot.SetServiceProvider(WebApplicationFactory.Services);

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
        const string sql = "DELETE FROM banks.internal_commands; " +
                           "DELETE FROM banks.outbox_messages; " +
                           "DELETE FROM banks.inbox_messages; " +
                           "DELETE FROM banks.banks; " +
                           "DELETE FROM banks.banks_synchronisation_processes; ";

        await connection.ExecuteScalarAsync(sql);
    }
}
