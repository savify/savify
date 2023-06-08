using System.Data;
using App.API;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Infrastructure.Configuration.Processing.Outbox;
using Dapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.Modules.Notifications.IntegrationTests.SeedWork;

public class TestBase
{
    protected CustomWebApplicationFactory<Program> WebApplicationFactory { get; private set; }
    
    protected INotificationsModule NotificationsModule { get; private set; }
    
    protected string ConnectionString { get; private set; }

    [OneTimeSetUp]
    public void Init()
    {
        WebApplicationFactory = new CustomWebApplicationFactory<Program>();
        
        using var scope = WebApplicationFactory.Services.CreateScope();
        NotificationsModule = scope.ServiceProvider.GetRequiredService<INotificationsModule>();
        ConnectionString = WebApplicationFactory.ConnectionString;
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
        const string sql = "DELETE FROM notifications.internal_commands; " +
                           "DELETE FROM notifications.outbox_messages; " +
                           "DELETE FROM notifications.inbox_messages; " +
                           "DELETE FROM notifications.user_notification_settings; ";

        await connection.ExecuteScalarAsync(sql);
    }
}
