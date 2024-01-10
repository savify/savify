using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Application.Incomes.RemoveIncome;

internal class RemoveIncomeCommandHandler(IIncomeRepository incomeRepository) : ICommandHandler<RemoveIncomeCommand>
{
    public async Task Handle(RemoveIncomeCommand command, CancellationToken cancellationToken)
    {
        var income = await incomeRepository.GetByIdAndUserIdAsync(new IncomeId(command.IncomeId), new UserId(command.UserId));

        income.Remove();

        incomeRepository.Remove(income);
    }
}
