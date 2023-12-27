using System.Data;
using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.BuildingBlocks.Integration;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace App.BuildingBlocks.Infrastructure.Configuration.EventBus;

public class IntegrationEventGenericHandler<TEvent>(DatabaseSchema schema) : IIntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
{
    public async Task Handle(TEvent @event)
    {
        using var scope = CompositionRoot.BeginScope();
        using var connection = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>().GetOpenConnection();

        var type = @event.GetType().FullName;
        var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver()
        });

        if (!ExistsWithId(@event.Id, connection).Result)
        {
            await InsertInboxMessage(@event, connection, type, data);
        }
    }

    private async Task InsertInboxMessage(TEvent @event, IDbConnection connection, string? type, string data)
    {
        var sql = $"INSERT INTO {schema.Name}.inbox_messages (id, occurred_on, type, data) " +
                  "VALUES (@Id, @OccurredOn, @Type, @Data)";

        await connection.ExecuteScalarAsync(sql, new
        {
            @event.Id,
            @event.OccurredOn,
            type,
            data
        });
    }

    private Task<bool> ExistsWithId(Guid eventId, IDbConnection connection)
    {
        var existsWithIdSql = $"SELECT EXISTS(SELECT 1 FROM {schema.Name}.inbox_messages WHERE id = @Id)";

        return connection.ExecuteScalarAsync<bool>(existsWithIdSql, new
        {
            Id = eventId
        });
    }
}
