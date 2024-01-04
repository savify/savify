using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;

public class GetCashWalletQuery(Guid walletId, Guid userId) : QueryBase<CashWalletDto?>
{
    public Guid WalletId { get; } = walletId;

    public Guid UserId { get; } = userId;
}
