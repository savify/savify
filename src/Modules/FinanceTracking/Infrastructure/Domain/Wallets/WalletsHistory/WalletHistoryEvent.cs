using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;

public class WalletHistoryEvent
{
    public Guid Id { get; private set; }

    public string Type { get; }

    public string Data { get; }

    public static WalletHistoryEvent From(IDomainEvent domainEvent)
    {
        var jsonData = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver()
        });

        return new WalletHistoryEvent(domainEvent.Id, MapDomainEventToType(domainEvent), jsonData);
    }

    public IDomainEvent ToDomainEvent()
    {
        var type = DomainEventTypeMappings.Dictionary[Type];

        return (JsonConvert.DeserializeObject(Data, type) as IDomainEvent)!;
    }

    private static string MapDomainEventToType(IDomainEvent domainEvent)
    {
        foreach (var key in DomainEventTypeMappings.Dictionary.Keys)
        {
            if (DomainEventTypeMappings.Dictionary[key] == domainEvent.GetType())
            {
                return key;
            }
        }

        throw new ArgumentException("Invalid Domain Event type", nameof(domainEvent));
    }

    private WalletHistoryEvent(Guid id, string type, string data)
    {
        Id = id;
        Type = type;
        Data = data;
    }

    private WalletHistoryEvent() { }
}
