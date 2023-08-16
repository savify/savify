using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.DebitWallets;

public interface IDebitWalletRepository
{
    Task AddAsync(DebitWallet wallet);

    Task<DebitWallet> GetByIdAsync(WalletId id);

    Task<DebitWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId);
}
