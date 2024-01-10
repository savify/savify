using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Incomes.Events;

public class IncomeRemovedDomainEvent(IncomeId incomeId, WalletId walletId, Money amount) : DomainEventBase
{
    public IncomeId IncomeId { get; } = incomeId;

    public WalletId WalletId { get; } = walletId;

    public Money Amount { get; } = amount;
}
