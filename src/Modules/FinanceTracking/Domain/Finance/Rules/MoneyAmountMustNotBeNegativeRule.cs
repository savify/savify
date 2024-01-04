using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Finance.Rules;

public class MoneyAmountMustNotBeNegativeRule(int amount) : IBusinessRule
{
    public bool IsBroken() => amount < 0;

    public string MessageTemplate => "Amount must not be negative";
}
