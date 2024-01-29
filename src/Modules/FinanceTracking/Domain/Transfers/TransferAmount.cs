using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Finance.Rules;

namespace App.Modules.FinanceTracking.Domain.Transfers;

public record TransferAmount
{
    public Money Source { get; }

    public Money Target { get; }

    public ExchangeRate ExchangeRate { get; }

    public static TransferAmount From(Money source, Money target)
    {
        var exchangeRate = ExchangeRate.For(source.Currency, target.Currency, (decimal)target.Amount / source.Amount);

        return new TransferAmount(source, target, exchangeRate);
    }

    public static TransferAmount From(Money source, ExchangeRate exchangeRate)
    {
        BusinessRuleChecker.CheckRules(new SourceCurrencyMustMatchExchangeRateFromCurrencyRule(source, exchangeRate));

        var target = Money.From((int)(source.Amount * exchangeRate.Rate), exchangeRate.To);

        return new TransferAmount(source, target, exchangeRate);
    }

    public static TransferAmount From(Money amount)
    {
        var exchangeRate = ExchangeRate.For(amount.Currency, amount.Currency, 1);

        return new TransferAmount(amount, amount, exchangeRate);
    }

    private TransferAmount(Money source, Money target, ExchangeRate exchangeRate)
    {
        Source = Money.From(source);
        Target = Money.From(target);
        ExchangeRate = ExchangeRate.For(exchangeRate);
    }

    private TransferAmount() { }
}
