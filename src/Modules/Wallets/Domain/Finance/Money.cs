namespace App.Modules.Wallets.Domain.Finance;

public record Money(decimal Amount, Currency Currency)
{
    public static Money From(decimal amount, Currency currency) => new(amount, currency);
    public static Money From(decimal amount, string currency) => new(amount, Currency.From(currency));
}
