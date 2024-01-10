using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Expenses;

public interface IExpenseRepository
{
    public Task AddAsync(Expense expense);

    public void Remove(Expense expense);

    public Task<Expense> GetByIdAsync(ExpenseId id);

    public Task<Expense> GetByIdAndUserIdAsync(ExpenseId id, UserId userId);
}
