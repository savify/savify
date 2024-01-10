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
        var income = await incomeRepository.GetByIdAndUserIdAsync(new IncomeId(command.IncomeId), new UserId(command.UserId));

        income.Edit(
            new WalletId(command.TargetWalletId),
            new CategoryId(command.CategoryId),
            Money.From(command.Amount, command.Currency),
            command.MadeOn,
            walletsRepository,
            command.Comment,
            command.Tags);
    }
}
