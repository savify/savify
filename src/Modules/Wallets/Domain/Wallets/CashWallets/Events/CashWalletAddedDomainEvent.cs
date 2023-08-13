using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.CashWallets.Events;

public class CashWalletAddedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }
    
    public UserId UserId { get; }
    
    public Currency Currency { get; }

    public CashWalletAddedDomainEvent(WalletId walletId, UserId userId, Currency currency)
    {
        WalletId = walletId;
        UserId = userId;
        Currency = currency;
    }
}
