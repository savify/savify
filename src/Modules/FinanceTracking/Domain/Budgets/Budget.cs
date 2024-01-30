using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Budgets.Events;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Budgets;

public class Budget : Entity, IAggregateRoot
{
    public BudgetId Id { get; private set; }

    public UserId UserId { get; private set; }

    private BudgetPeriod _period;

    private List<CategoryBudget> _categoriesBudget;

    public static Budget Add(UserId userId, BudgetPeriod period, IEnumerable<CategoryBudget> categoriesBudget)
    {
        return new Budget(userId, period, categoriesBudget);
    }

    public Budget CloneForPeriod(BudgetPeriod period)
    {
        return new Budget(UserId, period, _categoriesBudget);
    }

    public void Edit(BudgetPeriod newPeriod, IEnumerable<CategoryBudget> newCategoriesBudget)
    {
        _period = newPeriod;

        _categoriesBudget = newCategoriesBudget.ToList();

        AddDomainEvent(new BudgetEditedDomainEvent(Id, UserId));
    }

    public void Remove()
    {
        AddDomainEvent(new BudgetRemovedDomainEvent(Id, UserId));
    }

    private Budget(UserId userId, BudgetPeriod period, IEnumerable<CategoryBudget> categoriesBudget)
    {
        Id = new BudgetId(Guid.NewGuid());
        UserId = userId;
        _period = period;
        _categoriesBudget = categoriesBudget.ToList();

        AddDomainEvent(new BudgetAddedDomainEvent(Id, UserId));
    }

    private Budget() { }
}
