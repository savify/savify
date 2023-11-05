namespace App.Modules.Transactions.Domain.Finance;

public record Money
{
    public required int Amount { get; init; }
    public required Currency Currency { get; init; }

    public static Money From(int amount, Currency currency) => new() { Amount = amount, Currency = currency };
    public static Money From(int amount, string currency) => new() { Amount = amount, Currency = Currency.From(currency) };

    private Money()
    { }
}
