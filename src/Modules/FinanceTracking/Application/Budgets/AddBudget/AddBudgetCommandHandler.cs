using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Budgets;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Application.Budgets.AddBudget;

public class AddBudgetCommandHandler(IBudgetRepository budgetRepository) : ICommandHandler<AddBudgetCommand, Guid>
{
    public async Task<Guid> Handle(AddBudgetCommand command, CancellationToken cancellationToken)
    {
        var userId = new UserId(command.UserId);
        var budgetPeriod = BudgetPeriod.From(command.StartDate, command.EndDate);
        var categoryBudgets = command.CategoriesBudget.Select(c =>
            CategoryBudget.From(new CategoryId(c.Key), Money.From(c.Value, command.Currency)));

        var budget = Budget.Add(userId, budgetPeriod, categoryBudgets);

        await budgetRepository.AddAsync(budget);

        return budget.Id.Value;
    }
}
