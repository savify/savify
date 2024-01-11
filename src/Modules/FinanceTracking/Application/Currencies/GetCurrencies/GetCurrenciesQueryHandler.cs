using App.Modules.FinanceTracking.Application.Configuration.Queries;

namespace App.Modules.FinanceTracking.Application.Currencies.GetCurrencies;

public class GetCurrenciesQueryHandler(ICurrenciesProvider currenciesProvider) : IQueryHandler<GetCurrenciesQuery, string[]>
{
    public async Task<string[]> Handle(GetCurrenciesQuery query, CancellationToken cancellationToken)
    {
        var currencies = await currenciesProvider.GetCurrenciesAsync();

        return currencies.Select(c => c.Value).ToArray();
    }
}
