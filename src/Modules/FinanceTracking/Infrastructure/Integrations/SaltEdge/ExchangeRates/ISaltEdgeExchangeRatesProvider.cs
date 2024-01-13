namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ExchangeRates;

public interface ISaltEdgeExchangeRatesProvider
{
    public Task<IEnumerable<ExchangeRateDto>> FetchExchangeRatesAsync(DateTime? date = null);
}
