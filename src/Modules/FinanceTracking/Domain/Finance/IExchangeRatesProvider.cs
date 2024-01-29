namespace App.Modules.FinanceTracking.Domain.Finance;

public interface IExchangeRatesProvider
{
    Task<ExchangeRate> GetExchangeRateFor(Currency from, Currency to, DateTime? date = null);
}
