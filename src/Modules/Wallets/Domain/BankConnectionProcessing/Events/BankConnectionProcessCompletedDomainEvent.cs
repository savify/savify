using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Events;

public class BankConnectionProcessCompletedDomainEvent : DomainEventBase
{
    public BankConnectionProcessId BankConnectionProcessId { get; }

    public BankAccountId BankAccountId { get; }

    public BankConnectionProcessCompletedDomainEvent(BankConnectionProcessId bankConnectionProcessId, BankAccountId bankAccountId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
        BankAccountId = bankAccountId;
    }
}
