using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.RemoveCashWallet;

public class RemoveCashWalletCommand : CommandBase
{
    public Guid UserId { get; }

    public Guid WalletId { get; }

    public RemoveCashWalletCommand(Guid userId, Guid walletId)
    {
        UserId = userId;
        WalletId = walletId;
    }
}
