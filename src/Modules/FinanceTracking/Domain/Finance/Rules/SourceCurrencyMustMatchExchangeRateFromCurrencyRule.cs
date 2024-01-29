using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Finance.Rules;

public class SourceCurrencyMustMatchExchangeRateFromCurrencyRule(Money sourceAmount, ExchangeRate rate) : IBusinessRule
{
    public bool IsBroken() => sourceAmount.Currency != rate.From;

    public string MessageTemplate => "Source currency '{0}' doesn't match exchange rate currency '{1}'";

    public object[] MessageArguments => [sourceAmount.Currency.Value, rate.From.Value];
}
