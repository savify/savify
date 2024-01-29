using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Budgets.Rules;

public class BudgetPeriodEndDateCannotBeEarlierThanStartDateRule(DateOnly start, DateOnly end) : IBusinessRule
{
    public bool IsBroken() => end < start;

    public string MessageTemplate => "End date cannot be earlier than start date";
}
