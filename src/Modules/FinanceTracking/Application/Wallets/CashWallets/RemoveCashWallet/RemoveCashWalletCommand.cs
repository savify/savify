using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.RemoveCashWallet;

public class RemoveCashWalletCommand(Guid userId, Guid walletId) : CommandBase
{
    public Guid UserId { get; } = userId;

    public Guid WalletId { get; } = walletId;
}
