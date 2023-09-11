using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Finance;

namespace App.Modules.Wallets.Domain.Wallets.DebitWallets.Rules;

public class WalletFinanceDetailsCannotBeEditedIfBankAccountIsConnectedRule : IBusinessRule
{
    private readonly int? _newBalance;

    private readonly Currency? _newCurrency;

    private readonly bool _hasBankAccountConnected;

    public WalletFinanceDetailsCannotBeEditedIfBankAccountIsConnectedRule(int? newBalance, Currency? newCurrency, bool hasBankAccountConnected)
    {
        _newBalance = newBalance;
        _newCurrency = newCurrency;
        _hasBankAccountConnected = hasBankAccountConnected;
    }

    public bool IsBroken() => (_newBalance is not null || _newCurrency is not null) && _hasBankAccountConnected;

    public string MessageTemplate => "Wallet finance details cannot be changed when bank account is connected to wallet";
}
