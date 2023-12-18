using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;

public class BankConnectionProcessExpiredDomainEvent(BankConnectionProcessId bankConnectionProcessId) : DomainEventBase
{
    public BankConnectionProcessId BankConnectionProcessId { get; } = bankConnectionProcessId;
}
