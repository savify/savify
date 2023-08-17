using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.Wallets.DebitWallets.Rules;

public class DebitWalletCannotBeRemovedMoreThanOnceRule : IBusinessRule
{
    private readonly WalletId _walletId;

    private readonly bool _isRemoved;

    public DebitWalletCannotBeRemovedMoreThanOnceRule(WalletId walletId, bool isRemoved)
    {
        _walletId = walletId;
        _isRemoved = isRemoved;
    }

    public bool IsBroken() => _isRemoved;

    public string MessageTemplate => "Wallet with id '{0}' was already removed";

    public object[] MessageArguments => new object[] { _walletId.Value };
}
