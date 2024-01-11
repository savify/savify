namespace App.Modules.FinanceTracking.Domain.Finance;

public record Currency(string Value)
{
    public static Currency From(string value) => new(value);

    public static IEnumerable<Currency> DisabledCurrencies => new[]
    {
        From("RUB")
    };
}
