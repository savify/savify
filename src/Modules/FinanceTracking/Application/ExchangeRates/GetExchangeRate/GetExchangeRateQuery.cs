using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.ExchangeRates.GetExchangeRate;

public class GetExchangeRateQuery(string fromCurrencyCode, string toCurrencyCode) : QueryBase<decimal>
{
    public string FromCurrencyCode { get; } = fromCurrencyCode;

    public string ToCurrencyCode { get; } = toCurrencyCode;
}
