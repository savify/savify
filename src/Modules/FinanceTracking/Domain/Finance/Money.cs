using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance.Rules;

namespace App.Modules.FinanceTracking.Domain.Finance;

public record Money
{
    public int Amount { get; private init; }

    public Currency Currency { get; private init; }

    public static Money From(int amount, Currency currency) => new(amount, currency);

    public static Money From(int amount, string currency) => new(amount, Currency.From(currency));

    public static Money From(Money origin) => new(origin.Amount, origin.Currency);

    private Money(int amount, Currency currency)
    {
        BusinessRuleChecker.CheckRules(new MoneyAmountMustNotBeNegativeRule(amount));

        Amount = amount;
        Currency = Currency.From(currency);
    }

    private Money() { }
}
