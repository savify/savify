using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;

internal class AddNewExpenseCommandHandler(IExpenseRepository expenseRepository, IWalletsRepository walletsRepository) : ICommandHandler<AddNewExpenseCommand, Guid>
{
    public async Task<Guid> Handle(AddNewExpenseCommand command, CancellationToken cancellationToken)
    {
        var userId = new UserId(command.UserId);
        var sourceWalletId = new WalletId(command.SourceWalletId);

        var sourceWallet = await walletsRepository.GetByWalletIdAndUserIdAsync(sourceWalletId, userId);

        var expense = Expense.AddNew(
            userId,
            sourceWallet,
            new CategoryId(command.CategoryId),
            Money.From(command.Amount, sourceWallet.Currency),
            command.MadeOn,
            command.Comment,
            command.Tags);

        await expenseRepository.AddAsync(expense);

        return expense.Id.Value;
    }
}
