using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Rules;

public class CashWalletCannotBeChangedIfWasRemovedRule(WalletId walletId, bool isRemoved) : IBusinessRule
{
    public bool IsBroken() => isRemoved;

    public string MessageTemplate => "Wallet with id '{0}' was already removed and cannot be changed";

    public object[] MessageArguments => [walletId.Value];
}
