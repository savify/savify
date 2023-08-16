using System.Data;
using System.Reflection;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
using Dapper;
using MediatR;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.IntegrationTests.SeedWork;

public class OutboxMessagesAccessor
{
    public static async Task<List<OutboxMessageDto>> GetOutboxMessages(IDbConnection connection)
    {
        string sql = "SELECT " +
                     $"message.id as {nameof(OutboxMessageDto.Id)}, " +
                     $"message.type as {nameof(OutboxMessageDto.Type)}, " +
                     $"message.data as {nameof(OutboxMessageDto.Data)} " +
                     "FROM user_access.outbox_messages AS message " +
                     "ORDER BY message.occurred_on";

        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);

        return messages.AsList();
    }

    public static T Deserialize<T>(OutboxMessageDto message) where T : class, INotification
    {
        Type type = Assembly.GetAssembly(typeof(UserCreatedNotification))?.GetType(typeof(T).FullName);

        return JsonConvert.DeserializeObject(message.Data, type) as T;
    }
}
