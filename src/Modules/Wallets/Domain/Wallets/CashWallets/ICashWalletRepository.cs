using App.Modules.Wallets.Domain.Users;

namespace App.Modules.Wallets.Domain.Wallets.CashWallets;

public interface ICashWalletRepository
{
    Task AddAsync(CashWallet wallet);

    Task<CashWallet> GetByIdAsync(WalletId id);

    Task<CashWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId);
}
