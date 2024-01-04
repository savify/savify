using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Expenses.Events;

public class ExpenseAddedDomainEvent(ExpenseId expenseId, UserId userId, WalletId sourceWalletId, CategoryId categoryId, Money amount, IEnumerable<string> tags) : DomainEventBase
{
    public ExpenseId ExpenseId { get; } = expenseId;

    public UserId UserId { get; } = userId;

    public WalletId SourceWalletId { get; } = sourceWalletId;

    public CategoryId CategoryId { get; } = categoryId;

    public Money Amount { get; } = amount;

    public IEnumerable<string> Tags { get; } = tags;
}
