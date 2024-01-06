using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;

internal class AddNewIncomeCommandHandler(IIncomeRepository incomeRepository, IWalletsRepository walletsRepository) : ICommandHandler<AddNewIncomeCommand, Guid>
{
    public async Task<Guid> Handle(AddNewIncomeCommand command, CancellationToken cancellationToken)
    {
        var income = Income.AddNew(
            new UserId(command.UserId),
            new WalletId(command.TargetWalletId),
            new CategoryId(command.CategoryId),
            Money.From(command.Amount, command.Currency),
            command.MadeOn,
            walletsRepository,
            command.Comment,
            command.Tags);

        await incomeRepository.AddAsync(income);

        return income.Id.Value;
    }
}
