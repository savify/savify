using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Budgets;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Application.Budgets.RemoveBudget;

public class RemoveBudgetCommandHandler(IBudgetRepository budgetRepository) : ICommandHandler<RemoveBudgetCommand>
{
    public async Task Handle(RemoveBudgetCommand command, CancellationToken cancellationToken)
    {
        var budget = await budgetRepository.GetByIdAndUserIdAsync(new BudgetId(command.BudgetId), new UserId(command.UserId));

        budget.Remove();

        budgetRepository.Remove(budget);
    }
}
