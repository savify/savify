using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.CreditWallets.Events;

public class CreditWalletEditedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public CreditWalletEditedDomainEvent(WalletId walletId, UserId userId)
    {
        WalletId = walletId;
        UserId = userId;
    }
}
