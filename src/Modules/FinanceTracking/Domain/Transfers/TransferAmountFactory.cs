using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Domain.Transfers;

public class TransferAmountFactory(IExchangeRatesProvider exchangeRatesProvider)
{
    public async Task<TransferAmount> CreateAsync(int sourceAmount, Currency sourceCurrency, int? targetAmount, Currency targetCurrency, DateTime exchangeRateDate)
    {
        var source = Money.From(sourceAmount, sourceCurrency);

        if (sourceCurrency != targetCurrency && targetAmount is null)
        {
            var exchangeRate = await exchangeRatesProvider.GetExchangeRateFor(sourceCurrency, targetCurrency, exchangeRateDate);

            return TransferAmount.From(source, exchangeRate);
        }

        if (targetAmount is not null)
        {
            return TransferAmount.From(
                source,
                Money.From(targetAmount.Value, targetCurrency));
        }

        return TransferAmount.From(source);
    }
}
