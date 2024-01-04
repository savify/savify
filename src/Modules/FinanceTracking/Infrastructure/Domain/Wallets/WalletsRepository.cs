using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets;

public class WalletsRepository(FinanceTrackingContext financeTrackingContext) : IWalletsRepository
{
    public bool ExistsForUserIdAndWalletId(UserId userId, WalletId walletId)
    {
        return financeTrackingContext.CashWallets.Any(x => x.UserId == userId && x.Id == walletId) ||
               financeTrackingContext.DebitWallets.Any(x => x.UserId == userId && x.Id == walletId) ||
               financeTrackingContext.CreditWallets.Any(x => x.UserId == userId && x.Id == walletId);
    }
}
