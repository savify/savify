using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Budgets.Events;

public class BudgetRemovedDomainEvent(BudgetId budgetId, UserId userId) : DomainEventBase
{
    public BudgetId BudgetId { get; } = budgetId;

    public UserId UserId { get; } = userId;
}
