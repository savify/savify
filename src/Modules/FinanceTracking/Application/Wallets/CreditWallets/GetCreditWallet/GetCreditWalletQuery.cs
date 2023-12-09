using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;

public class GetCreditWalletQuery : QueryBase<CreditWalletDto?>
{
    public Guid WalletId { get; }

    public GetCreditWalletQuery(Guid walletId)
    {
        WalletId = walletId;
    }
}
