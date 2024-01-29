using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Domain.Wallets.Rules;

public class BalanceChangeAmountMustBeInTheWalletCurrencyRule(Money amount, Currency walletCurrency) : IBusinessRule
{
    public bool IsBroken() => amount.Currency != walletCurrency;

    public string MessageTemplate => "Currency '{0}' does not match wallet currency '{1}'";

    public object[] MessageArguments => [amount.Currency.Value, walletCurrency.Value];
}
