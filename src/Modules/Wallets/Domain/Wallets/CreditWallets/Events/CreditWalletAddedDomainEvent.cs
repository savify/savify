using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.CreditWallets.Events;

public class CreditWalletAddedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public Currency Currency { get; }

    public CreditWalletAddedDomainEvent(WalletId walletId, UserId userId, Currency currency)
    {
        WalletId = walletId;
        UserId = userId;
        Currency = currency;
    }
}
