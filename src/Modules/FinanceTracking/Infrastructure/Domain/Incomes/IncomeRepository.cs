using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Incomes;

public class IncomeRepository(FinanceTrackingContext financeTrackingContext) : IIncomeRepository
{
    public async Task AddAsync(Income income)
    {
        await financeTrackingContext.AddAsync(income);
    }

    public void Remove(Income income)
    {
        financeTrackingContext.Remove(income);
    }

    public async Task<Income> GetByIdAsync(IncomeId id)
    {
        var income = await financeTrackingContext.Incomes.SingleOrDefaultAsync(e => e.Id == id);

        if (income is null)
        {
            throw new NotFoundRepositoryException<Income>(id.Value);
        }

        return income;
    }

    public async Task<Income> GetByIdAndUserIdAsync(IncomeId id, UserId userId)
    {
        var income = await GetByIdAsync(id);

        if (income.UserId != userId)
        {
            throw new AccessDeniedException();
        }

        return income;
    }
}
