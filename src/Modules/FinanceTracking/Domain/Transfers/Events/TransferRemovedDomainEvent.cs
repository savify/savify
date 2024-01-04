using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Transfers.Events;
public class TransferRemovedDomainEvent : DomainEventBase
{
    public TransferId TransferId { get; }

    public TransferRemovedDomainEvent(TransferId transferId)
    {
        TransferId = transferId;
    }
}
