using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.CashWallets.Events;

public class CashWalletEditedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public CashWalletEditedDomainEvent(WalletId walletId, UserId userId)
    {
        WalletId = walletId;
        UserId = userId;
    }
}
