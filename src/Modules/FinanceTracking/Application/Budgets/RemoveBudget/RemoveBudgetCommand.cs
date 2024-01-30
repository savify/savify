using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Budgets.RemoveBudget;

public class RemoveBudgetCommand(Guid budgetId, Guid userId) : CommandBase
{
    public Guid BudgetId { get; } = budgetId;

    public Guid UserId { get; } = userId;
}
