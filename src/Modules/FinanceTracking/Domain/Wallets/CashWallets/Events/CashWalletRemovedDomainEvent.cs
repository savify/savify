using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Events;

public class CashWalletRemovedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public CashWalletRemovedDomainEvent(WalletId walletId, UserId userId)
    {
        WalletId = walletId;
        UserId = userId;
    }
}
