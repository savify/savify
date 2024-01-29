using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Expenses.EditExpense;

internal class EditExpenseCommandHandler(IExpenseRepository expenseRepository, IWalletsRepository walletsRepository) : ICommandHandler<EditExpenseCommand>
{
    public async Task Handle(EditExpenseCommand command, CancellationToken cancellationToken)
    {
        var userId = new UserId(command.UserId);
        var sourceWalletId = new WalletId(command.SourceWalletId);

        var sourceWallet = await walletsRepository.GetByWalletIdAndUserIdAsync(sourceWalletId, userId);

        var expense = await expenseRepository.GetByIdAndUserIdAsync(new ExpenseId(command.ExpenseId), new UserId(command.UserId));

        expense.Edit(
            sourceWallet,
            new CategoryId(command.CategoryId),
            Money.From(command.Amount, sourceWallet.Currency),
            command.MadeOn,
            command.Comment,
            command.Tags);
    }
}
