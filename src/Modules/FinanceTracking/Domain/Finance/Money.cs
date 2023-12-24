namespace App.Modules.FinanceTracking.Domain.Finance;

public record Money
{
    public int Amount { get; private init; }
    public Currency Currency { get; private init; }

    public static Money From(int amount, Currency currency) => new() { Amount = amount, Currency = currency };
    public static Money From(int amount, string currency) => new() { Amount = amount, Currency = Currency.From(currency) };

    private Money()
    { }
}
