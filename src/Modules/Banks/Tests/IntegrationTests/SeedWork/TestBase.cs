using System.Data;
using System.Reflection;
using App.API;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Database.Scripts.Clear;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Application.Contracts;
using App.Modules.Banks.Infrastructure;
using App.Modules.Banks.IntegrationTests.SeedData;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.Modules.Banks.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected SaltEdgeHttpClientMocker SaltEdgeHttpClientMocker { get; private set; }

    protected IBanksModule BanksModule { get; private set; }

    protected string ConnectionString { get; private set; }

    private static readonly Assembly ApplicationAssembly = Assembly.GetAssembly(typeof(CommandBase))!;

    [OneTimeSetUp]
    public async Task Init()
    {
        SaltEdgeHttpClientMocker = new SaltEdgeHttpClientMocker();

        WebApplicationFactory = await CustomWebApplicationFactory<Program>.Create(SaltEdgeHttpClientMocker.BaseUrl);
        CompositionRoot.SetServiceProvider(WebApplicationFactory.Services);

        ConnectionString = WebApplicationFactory.GetConnectionString();

        using var scope = WebApplicationFactory.Services.CreateScope();
        BanksModule = scope.ServiceProvider.GetRequiredService<IBanksModule>();

        var dbContext = scope.ServiceProvider.GetRequiredService<BanksContext>();
        await dbContext.Database.MigrateAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        SaltEdgeHttpClientMocker.StopWireMockServer();
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
        var messages = await OutboxMessagesAccessor.GetOutboxMessages(connection, DatabaseConfiguration.Schema, ApplicationAssembly);

        return messages;
    }

    protected async Task<T> GetLastOutboxMessage<T>() where T : class, INotification
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        var messages = await OutboxMessagesAccessor.GetOutboxMessages(connection, DatabaseConfiguration.Schema, ApplicationAssembly);

        return OutboxMessagesAccessor.Deserialize<T>(messages.Last(), ApplicationAssembly);
    }

    private static async Task ClearDatabase(IDbConnection connection)
    {
        var sql = await ClearDatabaseCommandProvider.GetAsync();

        await connection.ExecuteScalarAsync(sql);
    }
}
