using System.Data;
using System.Reflection;
using App.API;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Database.Scripts.Clear;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Infrastructure;
using App.Modules.FinanceTracking.IntegrationTests.SeedData;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WireMock.Server;

namespace App.Modules.FinanceTracking.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected SaltEdgeHttpClientMocker SaltEdgeHttpClientMocker { get; private set; }

    protected IFinanceTrackingModule FinanceTrackingModule { get; private set; }

    protected string ConnectionString { get; private set; }

    private static Assembly _applicationAssembly = Assembly.GetAssembly(typeof(CommandBase));

    [OneTimeSetUp]
    public async Task Init()
    {
        WebApplicationFactory = await CustomWebApplicationFactory<Program>.Create();
        CompositionRoot.SetServiceProvider(WebApplicationFactory.Services);

        ConnectionString = WebApplicationFactory.GetConnectionString();

        using var scope = WebApplicationFactory.Services.CreateScope();
        FinanceTrackingModule = scope.ServiceProvider.GetRequiredService<IFinanceTrackingModule>();

        SaltEdgeHttpClientMocker = new SaltEdgeHttpClientMocker(WireMockServer.StartWithAdminInterface(port: 1080, ssl: false));

        var dbContext = scope.ServiceProvider.GetRequiredService<FinanceTrackingContext>();
        await dbContext.Database.MigrateAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        SaltEdgeHttpClientMocker.StopWireMockServer();
        await WebApplicationFactory.StopDbContainerAsync();
        await WebApplicationFactory.DisposeAsync();
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
        var messages = await OutboxMessagesAccessor.GetOutboxMessages(connection, DatabaseConfiguration.Schema, _applicationAssembly);

        return messages;
    }

    protected async Task<T> GetLastOutboxMessage<T>() where T : class, INotification
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        var messages = await OutboxMessagesAccessor.GetOutboxMessages(connection, DatabaseConfiguration.Schema, _applicationAssembly);

        return OutboxMessagesAccessor.Deserialize<T>(messages.Last(), _applicationAssembly);
    }

    private static async Task ClearDatabase(IDbConnection connection)
    {
        var sql = await ClearDatabaseCommandProvider.GetAsync();

        await connection.ExecuteScalarAsync(sql);
    }
}
