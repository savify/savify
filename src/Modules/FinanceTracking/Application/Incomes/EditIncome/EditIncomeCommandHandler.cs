using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Incomes.EditIncome;

internal class EditIncomeCommandHandler(IIncomeRepository incomeRepository, IWalletsRepository walletsRepository) : ICommandHandler<EditIncomeCommand>
{
    public async Task Handle(EditIncomeCommand command, CancellationToken cancellationToken)
    {
        var userId = new UserId(command.UserId);
        var targetWalletId = new WalletId(command.TargetWalletId);
        var targetWallet = await walletsRepository.GetByWalletIdAndUserIdAsync(targetWalletId, userId);

        var income = await incomeRepository.GetByIdAndUserIdAsync(new IncomeId(command.IncomeId), new UserId(command.UserId));

        income.Edit(
            targetWallet,
            new CategoryId(command.CategoryId),
            Money.From(command.Amount, targetWallet.Currency),
            command.MadeOn,
            command.Comment,
            command.Tags);
    }
}
