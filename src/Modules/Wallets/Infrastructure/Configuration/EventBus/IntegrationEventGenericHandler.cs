using Dapper;
using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Infrastructure.Serialization;
using App.BuildingBlocks.Integration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace App.Modules.Wallets.Infrastructure.Configuration.EventBus;

internal class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T> where T : IntegrationEvent
{
    public async Task Handle(T @event)
    {
        using var scope = WalletsCompositionRoot.BeginScope();
        using var connection = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>().GetOpenConnection();
        
        string type = @event.GetType().FullName;
        var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver()
        });

        var sql = "INSERT INTO wallets.inbox_messages (id, occurred_on, type, data) " +
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
