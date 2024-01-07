using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

public interface ICashWalletRepository
{
    Task AddAsync(CashWallet wallet);

    Task UpdateHistoryAsync(CashWallet wallet);

    Task<CashWallet> GetByIdAsync(WalletId id);

    Task<CashWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId);
}
