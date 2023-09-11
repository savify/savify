using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Events;

public class BankConnectionProcessExpiredDomainEvent : DomainEventBase
{
    public BankConnectionProcessId BankConnectionProcessId { get; }

    public BankConnectionProcessExpiredDomainEvent(BankConnectionProcessId bankConnectionProcessId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
    }
}
