using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Expenses.RemoveExpense;

public class RemoveExpenseCommand(Guid expenseId, Guid userId) : CommandBase
{
    public Guid ExpenseId { get; } = expenseId;

    public Guid UserId { get; } = userId;
}
