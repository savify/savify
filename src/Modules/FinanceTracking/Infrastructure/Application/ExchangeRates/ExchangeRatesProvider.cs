using App.Modules.FinanceTracking.Application.ExchangeRates;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ExchangeRates;

namespace App.Modules.FinanceTracking.Infrastructure.Application.ExchangeRates;

public class ExchangeRatesProvider(ISaltEdgeExchangeRatesProvider saltEdgeExchangeRatesProvider) : IExchangeRatesProvider
{
    public async Task<ExchangeRate> GetExchangeRateFor(Currency from, Currency to, DateTime? date = null)
    {
        if (from == to)
        {
            return ExchangeRate.For(from, to, 1);
        }

        var exchangeRates = (await saltEdgeExchangeRatesProvider.FetchExchangeRatesAsync(date)).ToArray();

        var fromCurrencyExchangeRate = exchangeRates.Single(r => r.CurrencyCode == from.Value);
        var toCurrencyExchangeRate = exchangeRates.Single(r => r.CurrencyCode == to.Value);

        var rate = fromCurrencyExchangeRate.Rate / toCurrencyExchangeRate.Rate;

        return ExchangeRate.CalculateFor(from, to, fromCurrencyExchangeRate.Rate, toCurrencyExchangeRate.Rate);
    }
}
