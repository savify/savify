using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.CreditWallets;

public interface ICreditWalletRepository
{
    Task AddAsync(CreditWallet wallet);

    Task<CreditWallet> GetByIdAsync(WalletId id);

    Task<CreditWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId);
}
