using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;

public class GetDebitWalletQuery(Guid walletId, Guid userId) : QueryBase<DebitWalletDto?>
{
    public Guid WalletId { get; } = walletId;

    public Guid UserId { get; } = userId;
}
