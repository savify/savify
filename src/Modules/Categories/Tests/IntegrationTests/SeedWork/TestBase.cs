using System.Data;
using System.Reflection;
using App.API;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Database.Scripts.Clear;
using App.Modules.Categories.Application.Configuration.Data;
using App.Modules.Categories.Application.Contracts;
using App.Modules.Categories.Infrastructure;
using App.Modules.Categories.IntegrationTests.SeedData;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.Modules.Categories.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected SaltEdgeHttpClientMocker SaltEdgeHttpClientMocker { get; private set; }

    protected ICategoriesModule CategoriesModule { get; private set; }

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
        CategoriesModule = scope.ServiceProvider.GetRequiredService<ICategoriesModule>();

        var dbContext = scope.ServiceProvider.GetRequiredService<CategoriesContext>();
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
