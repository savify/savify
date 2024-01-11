namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Currencies;

public interface ISaltEdgeCurrenciesProvider
{
    public Task<IEnumerable<CurrencyDto>> FetchCurrenciesAsync();
}
