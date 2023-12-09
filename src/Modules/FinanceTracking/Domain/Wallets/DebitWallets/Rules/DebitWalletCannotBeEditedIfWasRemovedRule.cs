using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Rules;

public class DebitWalletCannotBeEditedIfWasRemovedRule : IBusinessRule
{
    private readonly WalletId _walletId;

    private readonly bool _isRemoved;

    public DebitWalletCannotBeEditedIfWasRemovedRule(WalletId walletId, bool isRemoved)
    {
        _walletId = walletId;
        _isRemoved = isRemoved;
    }

    public bool IsBroken() => _isRemoved;

    public string MessageTemplate => "Wallet with id '{0}' was already removed and cannot be edited";

    public object[] MessageArguments => new object[] { _walletId.Value };
}
