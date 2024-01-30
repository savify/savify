using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Budgets;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Application.Budgets.EditBudget;

public class EditBudgetCommandHandler(IBudgetRepository budgetRepository) : ICommandHandler<EditBudgetCommand>
{
    public async Task Handle(EditBudgetCommand command, CancellationToken cancellationToken)
    {
        var budget = await budgetRepository.GetByIdAndUserIdAsync(new BudgetId(command.BudgetId), new UserId(command.UserId));

        var budgetPeriod = BudgetPeriod.From(command.StartDate, command.EndDate);
        var categoryBudgets = command.CategoriesBudget.Select(c =>
            CategoryBudget.From(new CategoryId(c.Key), Money.From(c.Value, command.Currency)));

        budget.Edit(budgetPeriod, categoryBudgets);
    }
}
