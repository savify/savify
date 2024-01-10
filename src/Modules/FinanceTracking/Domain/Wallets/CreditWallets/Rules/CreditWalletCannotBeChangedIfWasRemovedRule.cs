using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Rules;

public class CreditWalletCannotBeChangedIfWasRemovedRule(WalletId walletId, bool isRemoved) : IBusinessRule
{
    public bool IsBroken() => isRemoved;

    public string MessageTemplate => "Wallet with id '{0}' was already removed and cannot be changed";

    public object[] MessageArguments => new object[] { walletId.Value };
}
