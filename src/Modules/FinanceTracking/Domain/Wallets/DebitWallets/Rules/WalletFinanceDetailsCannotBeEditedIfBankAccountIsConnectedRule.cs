using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Rules;

public class WalletFinanceDetailsCannotBeEditedIfBankAccountIsConnectedRule(
    int? newBalance,
    bool hasBankAccountConnected) : IBusinessRule
{
    public bool IsBroken() => newBalance is not null && hasBankAccountConnected;

    public string MessageTemplate => "Wallet finance details cannot be changed when bank account is connected to wallet";
}
