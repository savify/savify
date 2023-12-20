using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;

public class GetCashWalletQuery(Guid walletId) : QueryBase<CashWalletDto?>
{
    public Guid WalletId { get; } = walletId;
}
