using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using App.Modules.FinanceTracking.Application.Currencies;
using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Application.ExchangeRates.GetExchangeRate;

internal class GetExchangeRateQueryHandler(IExchangeRatesProvider exchangeRatesProvider, ICurrenciesProvider currenciesProvider) : IQueryHandler<GetExchangeRateQuery, decimal>
{
    public async Task<decimal> Handle(GetExchangeRateQuery query, CancellationToken cancellationToken)
    {
        // TODO: temporary solution; query validation should be introduced (like for commands)
        var supportedCurrencyCodes = (await currenciesProvider.GetCurrenciesAsync()).Select(c => c.Value).ToArray();
        if (!supportedCurrencyCodes.Contains(query.FromCurrencyCode) || !supportedCurrencyCodes.Contains(query.ToCurrencyCode))
        {
            throw new InvalidQueryException("From or To currency code is invalid");
        }

        var exchangeRate = await exchangeRatesProvider.GetExchangeRateFor(
            from: Currency.From(query.FromCurrencyCode),
            to: Currency.From(query.ToCurrencyCode));

        return exchangeRate.Rate;
    }
}
