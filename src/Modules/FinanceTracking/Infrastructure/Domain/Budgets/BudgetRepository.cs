using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Budgets;
using App.Modules.FinanceTracking.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Budgets;

public class BudgetRepository(FinanceTrackingContext financeTrackingContext) : IBudgetRepository
{
    public async Task AddAsync(Budget budget)
    {
        await financeTrackingContext.AddAsync(budget);
    }

    public async Task<Budget> GetByIdAsync(BudgetId id)
    {
        var budget = await financeTrackingContext.Budgets.SingleOrDefaultAsync(e => e.Id == id);

        if (budget is null)
        {
            throw new NotFoundRepositoryException<Budget>(id.Value);
        }

        return budget;
    }

    public async Task<Budget> GetByIdAndUserIdAsync(BudgetId id, UserId userId)
    {
        var budget = await GetByIdAsync(id);

        if (budget.UserId != userId)
        {
            throw new AccessDeniedException();
        }

        return budget;
    }
}
