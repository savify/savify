using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Application.Currencies;

public interface ICurrenciesProvider
{
    public Task<IEnumerable<Currency>> GetCurrenciesAsync();
}
