using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Budgets.EditBudget;

public class EditBudgetCommand(Guid budgetId, Guid userId, DateOnly startDate, DateOnly endDate, IDictionary<Guid, int> categoriesBudget, string currency) : CommandBase
{
    public Guid BudgetId { get; } = budgetId;

    public Guid UserId { get; } = userId;

    public DateOnly StartDate { get; } = startDate;

    public DateOnly EndDate { get; } = endDate;

    public IDictionary<Guid, int> CategoriesBudget { get; } = categoriesBudget;

    public string Currency { get; } = currency;
}
