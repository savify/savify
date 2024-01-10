using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;

public interface IWalletHistoryRepository
{
    Task AddAsync(WalletHistory walletHistory);

    Task<WalletHistory> GetByWalletIdAsync(WalletId walletId);
}
