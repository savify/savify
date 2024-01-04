using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Expenses.GetExpense;

public class GetExpenseQuery(Guid expenseId, Guid userId) : QueryBase<ExpenseDto?>
{
    public Guid ExpenseId { get; } = expenseId;

    public Guid UserId { get; set; } = userId;
}
