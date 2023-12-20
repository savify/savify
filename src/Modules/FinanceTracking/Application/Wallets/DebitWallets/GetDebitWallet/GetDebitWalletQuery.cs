using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;

public class GetDebitWalletQuery(Guid walletId) : QueryBase<DebitWalletDto?>
{
    public Guid WalletId { get; } = walletId;
}
