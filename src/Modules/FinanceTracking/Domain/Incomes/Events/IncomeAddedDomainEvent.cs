using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Domain.Incomes.Events;

public class IncomeAddedDomainEvent(IncomeId incomeId, UserId userId, WalletId targetWalletId, CategoryId categoryId, Money amount, IEnumerable<string> tags) : DomainEventBase
{
    public IncomeId IncomeId { get; } = incomeId;

    public UserId UserId { get; } = userId;

    public WalletId TargetWalletId { get; } = targetWalletId;

    public CategoryId CategoryId { get; } = categoryId;

    public Money Amount { get; } = amount;

    public IEnumerable<string> Tags { get; } = tags;
}
