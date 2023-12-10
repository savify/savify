using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Events;

public class DebitWalletEditedDomainEvent : DomainEventBase
{
    public WalletId WalletId { get; }

    public UserId UserId { get; }

    public Currency? NewCurrency { get; }

    public int? NewBalance { get; }

    public DebitWalletEditedDomainEvent(WalletId walletId, UserId userId, Currency? newCurrency, int? newBalance)
    {
        WalletId = walletId;
        UserId = userId;
        NewCurrency = newCurrency;
        NewBalance = newBalance;
    }
}
