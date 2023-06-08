using System.Data;
using System.Reflection;
using Dapper;
using App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;
using App.Modules.Notifications.Infrastructure.Configuration.Processing.Outbox;
using MediatR;
using Newtonsoft.Json;

namespace App.Modules.Notifications.IntegrationTests.SeedWork;

public class OutboxMessagesAccessor
{
    public static async Task<List<OutboxMessageDto>> GetOutboxMessages(IDbConnection connection)
    {
        string sql = "SELECT " +
                     $"message.id as {nameof(OutboxMessageDto.Id)}, " +
                     $"message.type as {nameof(OutboxMessageDto.Type)}, " +
                     $"message.data as {nameof(OutboxMessageDto.Data)} " +
                     "FROM notifications.outbox_messages AS message " +
                     "ORDER BY message.occurred_on";

        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);

        return messages.AsList();
    }

    public static T Deserialize<T>(OutboxMessageDto message) where T : class, INotification
    {
        // TODO: change to some notification if there'll be one
        Type type = Assembly.GetAssembly(typeof(CreateNotificationSettingsCommand))?.GetType(typeof(T).FullName);
        
        return JsonConvert.DeserializeObject(message.Data, type) as T;
    }
}
