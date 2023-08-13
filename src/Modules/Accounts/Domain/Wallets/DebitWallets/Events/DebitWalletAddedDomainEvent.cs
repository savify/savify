using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.DebitWallets.Events;

public class DebitWalletAddedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public Currency Currency { get; }

    public DebitWalletAddedDomainEvent(WalletId walletId, UserId userId, Currency currency)
    {
        WalletId = walletId;
        UserId = userId;
        Currency = currency;
    }
}