using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Expenses.Events;

public class ExpenseRemovedDomainEvent(ExpenseId expenseId, WalletId sourceWalletId, Money amount) : DomainEventBase
{
    public ExpenseId ExpenseId { get; } = expenseId;

    public WalletId SourceWalletId { get; } = sourceWalletId;

    public Money Amount { get; } = amount;
}
