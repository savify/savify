using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.BankConnections.Events;

public class BankConnectionCreatedDomainEvent : DomainEventBase
{
    public BankConnectionId BankConnectionId { get; }

    public BankId BankId { get; }

    public UserId UserId { get; }

    public BankConnectionCreatedDomainEvent(BankConnectionId bankConnectionId, BankId bankId, UserId userId)
    {
        BankConnectionId = bankConnectionId;
        BankId = bankId;
        UserId = userId;
    }
}
