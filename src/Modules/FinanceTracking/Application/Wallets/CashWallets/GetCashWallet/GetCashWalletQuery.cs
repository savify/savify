using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;

public class GetCashWalletQuery : QueryBase<CashWalletDto?>
{
    public Guid WalletId { get; }

    public GetCashWalletQuery(Guid walletId)
    {
        WalletId = walletId;
    }
}
