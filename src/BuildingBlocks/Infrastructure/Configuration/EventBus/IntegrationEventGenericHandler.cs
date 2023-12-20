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
}
