using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.Banks.BankRevisions.Events;

public class BankRevisionCreatedDomainEvent : DomainEventBase
{
    public BankRevisionId BankRevisionId { get; }

    public BankId BankId { get; }

    public BankRevisionType BankRevisionType { get; }

    public BankRevisionCreatedDomainEvent(BankRevisionId bankRevisionId, BankId bankId, BankRevisionType bankRevisionType)
    {
        BankRevisionId = bankRevisionId;
        BankId = bankId;
        BankRevisionType = bankRevisionType;
    }
}
