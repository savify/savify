using System.Data;
using System.Reflection;
using App.Modules.Wallets.Application.Configuration.Data;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Infrastructure.Configuration.Processing.Outbox;
using Dapper;
using MediatR;
using Newtonsoft.Json;

namespace App.Modules.Wallets.IntegrationTests.SeedWork;

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
        // TODO: change to some notification if there'll be one
        Type type = Assembly.GetAssembly(typeof(CommandBase))?.GetType(typeof(T).FullName);

        return JsonConvert.DeserializeObject(message.Data, type) as T;
    }
}
