using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Domain.Wallets.Events;

public class WalletBalanceDecreasedDomainEvent(WalletId walletId, Money amount, int newBalance) : DomainEventBase, IWalletHistoryDomainEvent
{
    public WalletId WalletId { get; } = walletId;

    public Money Amount { get; } = amount;

    public int NewBalance { get; } = newBalance;
}
