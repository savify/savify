using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Events;

public class BankConnectionProcessInitiatedDomainEvent : DomainEventBase
{
    public BankConnectionProcessId BankConnectionProcessId { get; }

    public UserId UserId { get; }

    public BankId BankId { get; }

    public BankConnectionProcessInitiatedDomainEvent(BankConnectionProcessId bankConnectionProcessId, UserId userId, BankId bankId)
    {
        BankConnectionProcessId = bankConnectionProcessId;
        UserId = userId;
        BankId = bankId;
    }
}
