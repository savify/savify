using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

public interface IDebitWalletRepository
{
    Task AddAsync(DebitWallet wallet);

    Task UpdateHistoryAsync(DebitWallet wallet);

    Task<DebitWallet> GetByIdAsync(WalletId id);

    Task<DebitWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId);
}
