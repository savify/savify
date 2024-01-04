using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;

public class AddNewExpenseCommandHandler(IExpenseRepository expenseRepository, IWalletsRepository walletsRepository) : ICommandHandler<AddNewExpenseCommand, Guid>
{
    public async Task<Guid> Handle(AddNewExpenseCommand command, CancellationToken cancellationToken)
    {
        var expense = Expense.AddNew(
            new UserId(command.UserId),
            new WalletId(command.SourceWalletId),
            new CategoryId(command.CategoryId),
            Money.From(command.Amount, command.Currency),
            command.MadeOn,
            walletsRepository,
            command.Comment,
            command.Tags);

        await expenseRepository.AddAsync(expense);

        return expense.Id.Value;
    }
}
