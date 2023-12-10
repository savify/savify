using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.RemoveDebitWallet;

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
