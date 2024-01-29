namespace App.Modules.FinanceTracking.Domain.Finance;

public record ExchangeRate
{
    public Currency From { get; init; }

    public Currency To { get; init; }

    public decimal Rate { get; init; }

    public static ExchangeRate For(Currency from, Currency to, decimal rate)
    {
        return new ExchangeRate(from, to, Math.Round(rate, 2));
    }

    public static ExchangeRate CalculateFor(Currency from, Currency to, decimal fromToUsd, decimal toToUsd)
    {
        return new ExchangeRate(from, to, Math.Round(fromToUsd / toToUsd, 2));
    }

    public static ExchangeRate For(ExchangeRate origin) => new(origin.From, origin.To, origin.Rate);

    private ExchangeRate(Currency from, Currency to, decimal rate)
    {
        From = Currency.From(from);
        To = Currency.From(to);
        Rate = rate;
    }

    private ExchangeRate() { }
}
