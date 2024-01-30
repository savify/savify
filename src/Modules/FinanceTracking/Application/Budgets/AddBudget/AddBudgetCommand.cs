using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Budgets.AddBudget;

public class AddBudgetCommand(Guid userId, DateOnly startDate, DateOnly endDate, IDictionary<Guid, int> categoriesBudget, string currency) : CommandBase<Guid>
{
    public Guid UserId { get; } = userId;

    public DateOnly StartDate { get; } = startDate;

    public DateOnly EndDate { get; } = endDate;

    public IDictionary<Guid, int> CategoriesBudget { get; } = categoriesBudget;

    public string Currency { get; } = currency;
}
