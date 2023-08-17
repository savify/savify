using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.RemoveCashWallet;

public class RemoveCashWalletCommand : CommandBase<Result>
{
    public Guid UserId { get; }

    public Guid WalletId { get; }

    public RemoveCashWalletCommand(Guid userId, Guid walletId)
    {
        UserId = userId;
        WalletId = walletId;
    }
}
