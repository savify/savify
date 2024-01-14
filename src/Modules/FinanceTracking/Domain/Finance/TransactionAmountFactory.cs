namespace App.Modules.FinanceTracking.Domain.Finance;

public class TransactionAmountFactory(IExchangeRatesProvider exchangeRatesProvider)
{
    public async Task<TransactionAmount> CreateAsync(int sourceAmount, Currency sourceCurrency, int? targetAmount, Currency targetCurrency, DateTime exchangeRateDate)
    {
        var source = Money.From(sourceAmount, sourceCurrency);

        if (sourceCurrency != targetCurrency && targetAmount is null)
        {
            var exchangeRate = await exchangeRatesProvider.GetExchangeRateFor(sourceCurrency, targetCurrency, exchangeRateDate);

            return TransactionAmount.From(source, exchangeRate);
        }

        if (targetAmount is not null)
        {
            return TransactionAmount.From(
                source,
                Money.From(targetAmount.Value, targetCurrency));
        }

        return TransactionAmount.From(source);
    }
}
