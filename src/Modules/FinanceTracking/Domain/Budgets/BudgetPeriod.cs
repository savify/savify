using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Budgets.Rules;

namespace App.Modules.FinanceTracking.Domain.Budgets;

public record BudgetPeriod
{
    public DateOnly Start { get; private set; }

    public DateOnly End { get; private set; }

    public static BudgetPeriod From(DateOnly start, DateOnly end)
    {
        return new BudgetPeriod(start, end);
    }

    public static BudgetPeriod From(DateTime start, DateTime end)
    {
        return new BudgetPeriod(DateOnly.FromDateTime(start), DateOnly.FromDateTime(end));
    }

    private BudgetPeriod(DateOnly start, DateOnly end)
    {
        BusinessRuleChecker.CheckRules(new BudgetPeriodEndDateCannotBeEarlierThanStartDateRule(start, end));

        Start = start;
        End = end;
    }

    private BudgetPeriod() { }
}
