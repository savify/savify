using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets;

public interface IWalletsRepository
{
    bool ExistsForUserIdAndWalletId(UserId userId, WalletId walletId);

    Task<Wallet> GetByWalletIdAsync(WalletId walletId);

    Task UpdateHistoryAsync(Wallet wallet);
}
