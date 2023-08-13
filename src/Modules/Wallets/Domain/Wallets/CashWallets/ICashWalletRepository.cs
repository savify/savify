namespace App.Modules.Wallets.Domain.Wallets.CashWallets;

public interface ICashWalletRepository
{
    Task AddAsync(CashWallet wallet);

    Task<CashWallet> GetByIdAsync(WalletId id);
}
