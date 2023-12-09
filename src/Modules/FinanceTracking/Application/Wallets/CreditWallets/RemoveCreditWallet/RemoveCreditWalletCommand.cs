using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.RemoveCreditWallet;

public class RemoveCreditWalletCommand : CommandBase
{
    public Guid UserId { get; }

    public Guid WalletId { get; }

    public RemoveCreditWalletCommand(Guid userId, Guid walletId)
    {
        UserId = userId;
        WalletId = walletId;
    }
}
