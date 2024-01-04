using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Expenses.EditExpense;

public class EditExpenseCommandHandler(IExpenseRepository expenseRepository, IWalletsRepository walletsRepository) : ICommandHandler<EditExpenseCommand>
{
    public async Task Handle(EditExpenseCommand command, CancellationToken cancellationToken)
    {
        var expense = await expenseRepository.GetByIdAndUserIdAsync(new ExpenseId(command.ExpenseId), new UserId(command.UserId));

        expense.Edit(
            new WalletId(command.SourceWalletId),
            new CategoryId(command.CategoryId),
            Money.From(command.Amount, command.Currency),
            command.MadeOn,
            walletsRepository,
            command.Comment,
            command.Tags);
    }
}
