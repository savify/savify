using App.Modules.FinanceTracking.Domain.Budgets;
using App.Modules.FinanceTracking.Domain.Budgets.Rules;

namespace App.Modules.FinanceTracking.UnitTests.Budgets;

[TestFixture]
public class BudgetPeriodTests : UnitTestBase
{
    [Test]
    public void CreatingBudgetPeriod_FromDateTime_IsSuccessful()
    {
        var period = BudgetPeriod.From(DateTime.Parse("2022-01-01"), DateTime.Parse("2022-01-31"));

        Assert.That(period.Start, Is.EqualTo(DateOnly.FromDateTime(DateTime.Parse("2022-01-01"))));
        Assert.That(period.End, Is.EqualTo(DateOnly.FromDateTime(DateTime.Parse("2022-01-31"))));
    }

    [Test]
    public void CreatingBudgetPeriod_FromDateOnly_IsSuccessful()
    {
        var period = BudgetPeriod.From(DateOnly.FromDateTime(DateTime.Parse("2022-01-01")), DateOnly.FromDateTime(DateTime.Parse("2022-01-31")));

        Assert.That(period.Start, Is.EqualTo(DateOnly.FromDateTime(DateTime.Parse("2022-01-01"))));
        Assert.That(period.End, Is.EqualTo(DateOnly.FromDateTime(DateTime.Parse("2022-01-31"))));
    }

    [Test]
    public void CreatingBudgetPeriod_WhenStartDateIsAfterEndDate_BreaksBudgetPeriodEndDateCannotBeEarlierThanStartDateRule()
    {
        AssertBrokenRule<BudgetPeriodEndDateCannotBeEarlierThanStartDateRule>(() =>
        {
            _ = BudgetPeriod.From(DateTime.Parse("2022-01-31"), DateTime.Parse("2022-01-01"));
        });
    }
}
