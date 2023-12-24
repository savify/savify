using System.Data;
using System.Reflection;
using App.API;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Database.Scripts.Clear;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Infrastructure;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.Modules.UserAccess.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }

    protected IUserAccessModule UserAccessModule { get; private set; }

    protected string ConnectionString { get; private set; }

    protected IDomainNotificationsMapper<UserAccessContext> DomainNotificationsMapper { get; private set; }

    private static readonly Assembly ApplicationAssembly = Assembly.GetAssembly(typeof(CommandBase))!;

    [OneTimeSetUp]
    public async Task Init()
    {
        WebApplicationFactory = await CustomWebApplicationFactory<Program>.Create();
        CompositionRoot.SetServiceProvider(WebApplicationFactory.Services);

        ConnectionString = WebApplicationFactory.GetConnectionString();

        using var scope = WebApplicationFactory.Services.CreateScope();
        UserAccessModule = scope.ServiceProvider.GetRequiredService<IUserAccessModule>();
        DomainNotificationsMapper = scope.ServiceProvider.GetRequiredService<IDomainNotificationsMapper<UserAccessContext>>();

        var dbContext = scope.ServiceProvider.GetRequiredService<UserAccessContext>();
        await dbContext.Database.MigrateAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
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
        var messages = await OutboxMessagesAccessor.GetOutboxMessages(connection, DatabaseConfiguration.Schema, ApplicationAssembly);

        return messages;
    }

    protected async Task<T> GetSingleOutboxMessage<T>() where T : class, INotification
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        var notificationType = DomainNotificationsMapper.GetName(typeof(T));
        var messages = await OutboxMessagesAccessor.GetOutboxMessages(connection, DatabaseConfiguration.Schema, ApplicationAssembly);
        var message = messages.Single(m => m.Type == notificationType);

        return OutboxMessagesAccessor.Deserialize<T>(message, ApplicationAssembly);
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
