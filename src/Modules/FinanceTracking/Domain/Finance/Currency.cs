namespace App.Modules.FinanceTracking.Domain.Finance;

public record Currency
{
    public string Value { get; }

    public static Currency From(string value) => new(value);

    public static IEnumerable<Currency> DisabledCurrencies => new[]
    {
        From("RUB")
    };

    private Currency(string value)
    {
        Value = value;
    }

    private Currency() { }
}
