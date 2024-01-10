using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

public interface ICreditWalletRepository
{
    Task AddAsync(CreditWallet wallet);

    Task UpdateHistoryAsync(CreditWallet wallet);

    Task<CreditWallet> GetByIdAsync(WalletId id);

    Task<CreditWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId);
}
