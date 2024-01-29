namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ExchangeRates;

public class ExchangeRateDto(string currencyCode, decimal rate)
{
    public string CurrencyCode { get; } = currencyCode;

    public decimal Rate { get; } = rate;
}
