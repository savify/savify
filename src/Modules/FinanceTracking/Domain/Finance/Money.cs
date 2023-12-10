namespace App.Modules.FinanceTracking.Domain.Finance;

public record Money
{
    public required decimal Amount { get; init; }
    public required Currency Currency { get; init; }

    public static Money From(decimal amount, Currency currency) => new() { Amount = amount, Currency = currency };
    public static Money From(decimal amount, string currency) => new() { Amount = amount, Currency = Currency.From(currency) };

    private Money()
    { }
}
