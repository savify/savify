using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;

public class GetDebitWalletQuery : QueryBase<DebitWalletDto?>
{
    public Guid WalletId { get; }

    public GetDebitWalletQuery(Guid walletId)
    {
        WalletId = walletId;
    }
}
