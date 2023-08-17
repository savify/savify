using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.RemoveCreditWallet;

public class RemoveCreditWalletCommand : CommandBase<Result>
{
    public Guid UserId { get; }

    public Guid WalletId { get; }

    public RemoveCreditWalletCommand(Guid userId, Guid walletId)
    {
        UserId = userId;
        WalletId = walletId;
    }
}
