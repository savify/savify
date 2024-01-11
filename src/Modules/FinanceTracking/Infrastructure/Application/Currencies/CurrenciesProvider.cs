using App.Modules.FinanceTracking.Application.Currencies;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Currencies;

namespace App.Modules.FinanceTracking.Infrastructure.Application.Currencies;

public class CurrenciesProvider(ISaltEdgeCurrenciesProvider saltEdgeCurrenciesProvider) : ICurrenciesProvider
{
    public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
    {
        // TODO: add caching - https://savify.atlassian.net/browse/SAV-172
        var saltEdgeCurrencies = await saltEdgeCurrenciesProvider.FetchCurrenciesAsync();

        return saltEdgeCurrencies.Select(c => Currency.From(c.Code)).Where(c => !Currency.DisabledCurrencies.Contains(c));
    }
}
