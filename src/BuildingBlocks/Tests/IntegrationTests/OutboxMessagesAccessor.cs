using System.Data;
using System.Reflection;
using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using Dapper;
using MediatR;
using Newtonsoft.Json;

namespace App.BuildingBlocks.Tests.IntegrationTests;

public static class OutboxMessagesAccessor
{
    public static async Task<List<OutboxMessageDto>> GetOutboxMessages(IDbConnection connection, DatabaseSchema schema, Assembly notificationsAssembly)
    {
        var sql = $"""
                   SELECT
                       message.id as {nameof(OutboxMessageDto.Id)},
                       message.type as {nameof(OutboxMessageDto.Type)},
                       message.data as {nameof(OutboxMessageDto.Data)}
                   FROM {schema.Name}.outbox_messages AS message
                   ORDER BY message.occurred_on
                   """;

        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);

        return messages.AsList();
    }

    public static T Deserialize<T>(OutboxMessageDto message, Assembly notificationsAssembly) where T : class, INotification
    {
        Type type = notificationsAssembly.GetType(typeof(T).FullName);

        return JsonConvert.DeserializeObject(message.Data, type) as T;
    }
}
