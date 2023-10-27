using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.RemoveDebitWallet;

public class RemoveDebitWalletCommand : CommandBase
{
    public Guid UserId { get; }

    public Guid WalletId { get; }

    public RemoveDebitWalletCommand(Guid userId, Guid walletId)
    {
        UserId = userId;
        WalletId = walletId;
    }
}
