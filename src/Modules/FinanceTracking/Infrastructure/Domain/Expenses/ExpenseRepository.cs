using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Expenses;

public class ExpenseRepository(FinanceTrackingContext financeTrackingContext) : IExpenseRepository
{
    public async Task AddAsync(Expense expense)
    {
        await financeTrackingContext.AddAsync(expense);
    }

    public async Task<Expense> GetByIdAsync(ExpenseId id)
    {
        var expense = await financeTrackingContext.Expenses.SingleOrDefaultAsync(e => e.Id == id);

        if (expense is null)
        {
            throw new NotFoundRepositoryException<Expense>(id.Value);
        }

        return expense;
    }

    public async Task<Expense> GetByIdAndUserIdAsync(ExpenseId id, UserId userId)
    {
        var expense = await GetByIdAsync(id);

        if (expense.UserId != userId)
        {
            throw new AccessDeniedException();
        }

        return expense;
    }
}
