using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.GetCreditWallet;

public class GetCreditWalletQuery : QueryBase<CreditWalletDto?>
{
    public Guid WalletId { get; }

    public GetCreditWalletQuery(Guid walletId)
    {
        WalletId = walletId;
    }
}
