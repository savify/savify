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
        var userId = new UserId(command.UserId);
        var targetWalletId = new WalletId(command.TargetWalletId);

        var targetWallet = await walletsRepository.GetByWalletIdAndUserIdAsync(targetWalletId, userId);

        var income = Income.AddNew(
            userId,
            targetWallet,
            new CategoryId(command.CategoryId),
            Money.From(command.Amount, targetWallet.Currency),
            command.MadeOn,
            command.Comment,
            command.Tags);

        await incomeRepository.AddAsync(income);

        return income.Id.Value;
    }
}
