using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.DebitWallets.Events;

public class DebitWalletRemovedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public DebitWalletRemovedDomainEvent(WalletId walletId, UserId userId)
    {
        WalletId = walletId;
        UserId = userId;
    }
}
