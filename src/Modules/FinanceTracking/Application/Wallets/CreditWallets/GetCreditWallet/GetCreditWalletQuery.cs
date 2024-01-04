using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;

public class GetCreditWalletQuery(Guid walletId, Guid userId) : QueryBase<CreditWalletDto?>
{
    public Guid WalletId { get; } = walletId;

    public Guid UserId { get; } = userId;
}
