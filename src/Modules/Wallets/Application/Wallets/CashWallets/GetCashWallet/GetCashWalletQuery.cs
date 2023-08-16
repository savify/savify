using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.GetCashWallet;

public class GetCashWalletQuery : QueryBase<CashWalletDto?>
{
    public Guid WalletId { get; }

    public GetCashWalletQuery(Guid walletId)
    {
        WalletId = walletId;
    }
}
