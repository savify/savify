using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Application.Expenses.RemoveExpense;

public class RemoveExpenseCommandHandler(IExpenseRepository expenseRepository) : ICommandHandler<RemoveExpenseCommand>
{
    public async Task Handle(RemoveExpenseCommand command, CancellationToken cancellationToken)
    {
        var expense = await expenseRepository.GetByIdAndUserIdAsync(new ExpenseId(command.ExpenseId), new UserId(command.UserId));

        expense.Remove();

        expenseRepository.Remove(expense);
    }
}
