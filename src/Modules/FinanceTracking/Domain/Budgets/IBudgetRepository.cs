using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Budgets;

public interface IBudgetRepository
{
    Task AddAsync(Budget budget);

    void Remove(Budget budget);

    Task<Budget> GetByIdAsync(BudgetId id);

    Task<Budget> GetByIdAndUserIdAsync(BudgetId id, UserId userId);
}
