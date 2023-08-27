namespace App.Modules.Wallets.Domain.Finance;

internal record Money(decimal amount, Currency currency)
{
    public static Money From(decimal amount, Currency currency) => new Money(amount, currency);
    public static Money From(decimal amount, string currency) => new Money(amount, Currency.From(currency));
}
