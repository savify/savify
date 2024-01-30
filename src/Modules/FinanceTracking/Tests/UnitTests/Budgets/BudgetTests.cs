using App.Modules.FinanceTracking.Domain.Budgets;
using App.Modules.FinanceTracking.Domain.Budgets.Events;
using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.UnitTests.Budgets;

[TestFixture]
public class BudgetTests : UnitTestBase
{
    [Test]
    public void AddingNewBudget_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var budgetPeriod = BudgetPeriod.From(DateTime.Parse("2022-01-01"), DateTime.Parse("2022-01-31"));
        var categoriesBudget = new List<CategoryBudget>
        {
            CategoryBudget.From(new CategoryId(Guid.NewGuid()), Money.From(100, Currency.From("PLN"))),
            CategoryBudget.From(new CategoryId(Guid.NewGuid()), Money.From(100, Currency.From("PLN"))),
        };

        var budget = Budget.Add(userId, budgetPeriod, categoriesBudget);

        var budgetAddedDomainEvent = AssertPublishedDomainEvent<BudgetAddedDomainEvent>(budget);
        Assert.That(budgetAddedDomainEvent.BudgetId, Is.EqualTo(budget.Id));
        Assert.That(budgetAddedDomainEvent.UserId, Is.EqualTo(userId));
    }

    [Test]
    public void EditingBudget_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var budgetPeriod = BudgetPeriod.From(DateTime.Parse("2022-01-01"), DateTime.Parse("2022-01-31"));
        var categoriesBudget = new List<CategoryBudget>
        {
            CategoryBudget.From(new CategoryId(Guid.NewGuid()), Money.From(100, Currency.From("PLN")))
        };

        var budget = Budget.Add(userId, budgetPeriod, categoriesBudget);

        var newBudgetPeriod = BudgetPeriod.From(DateTime.Parse("2022-02-01"), DateTime.Parse("2022-02-28"));
        var newCategoriesBudget = new List<CategoryBudget>
        {
            CategoryBudget.From(new CategoryId(Guid.NewGuid()), Money.From(100, Currency.From("PLN")))
        };

        budget.Edit(newBudgetPeriod, newCategoriesBudget);

        var budgetEditedDomainEvent = AssertPublishedDomainEvent<BudgetEditedDomainEvent>(budget);
        Assert.That(budgetEditedDomainEvent.BudgetId, Is.EqualTo(budget.Id));
        Assert.That(budgetEditedDomainEvent.UserId, Is.EqualTo(userId));
    }

    [Test]
    public void RemovingBudget_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var budgetPeriod = BudgetPeriod.From(DateTime.Parse("2022-01-01"), DateTime.Parse("2022-01-31"));
        var categoriesBudget = new List<CategoryBudget>
        {
            CategoryBudget.From(new CategoryId(Guid.NewGuid()), Money.From(100, Currency.From("PLN")))
        };

        var budget = Budget.Add(userId, budgetPeriod, categoriesBudget);

        budget.Remove();

        var budgetRemovedDomainEvent = AssertPublishedDomainEvent<BudgetRemovedDomainEvent>(budget);
        Assert.That(budgetRemovedDomainEvent.BudgetId, Is.EqualTo(budget.Id));
        Assert.That(budgetRemovedDomainEvent.UserId, Is.EqualTo(userId));
    }
}
