using System.Data;
using System.Reflection;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.Modules.Categories.Application.Configuration.Data;
using App.Modules.Categories.Application.Contracts;
using Dapper;
using MediatR;
using Newtonsoft.Json;

namespace App.Modules.Categories.IntegrationTests.SeedWork;

public static class OutboxMessagesAccessor
{
    public static async Task<List<OutboxMessageDto>> GetOutboxMessages(IDbConnection connection)
    {
        var sql = $"""
                   SELECT
                       message.id as {nameof(OutboxMessageDto.Id)},
                       message.type as {nameof(OutboxMessageDto.Type)},
                       message.data as {nameof(OutboxMessageDto.Data)}
                   FROM {DatabaseConfiguration.Schema.Name}.outbox_messages AS message
                   ORDER BY message.occurred_on
                   """;

        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);

        return messages.AsList();
    }

    public static T Deserialize<T>(OutboxMessageDto message) where T : class, INotification
    {
        Type type = Assembly.GetAssembly(typeof(CommandBase))?.GetType(typeof(T).FullName);

        return JsonConvert.DeserializeObject(message.Data, type) as T;
    }
}
