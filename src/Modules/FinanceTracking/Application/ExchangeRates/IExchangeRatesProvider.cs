using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Application.ExchangeRates;

public interface IExchangeRatesProvider
{
    Task<ExchangeRate> GetExchangeRateFor(Currency from, Currency to, DateTime? date = null);
}
