using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Events;

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
