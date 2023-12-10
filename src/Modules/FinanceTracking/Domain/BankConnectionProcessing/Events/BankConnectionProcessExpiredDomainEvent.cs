using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;

public class BankConnectionProcessExpiredDomainEvent : DomainEventBase
{
    public BankConnectionProcessId BankConnectionProcessId { get; }

    public BankConnectionProcessExpiredDomainEvent(BankConnectionProcessId bankConnectionProcessId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
    }
}
