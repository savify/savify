using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.GetDebitWallet;

public class GetDebitWalletQuery : QueryBase<DebitWalletDto?>
{
    public Guid WalletId { get; }

    public GetDebitWalletQuery(Guid walletId)
    {
        WalletId = walletId;
    }
}
