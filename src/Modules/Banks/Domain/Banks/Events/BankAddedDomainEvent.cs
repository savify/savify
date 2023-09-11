using App.BuildingBlocks.Domain;
using App.Modules.Banks.Domain.ExternalProviders;

namespace App.Modules.Banks.Domain.Banks.Events;

public class BankAddedDomainEvent : DomainEventBase
{
    public BankId BankId { get; }

    public ExternalProviderName ExternalProviderName { get; }

    public string ExternalId { get; }

    public BankAddedDomainEvent(BankId bankId, ExternalProviderName externalProviderName, string externalId)
    {
        BankId = bankId;
        ExternalProviderName = externalProviderName;
        ExternalId = externalId;
    }
}
