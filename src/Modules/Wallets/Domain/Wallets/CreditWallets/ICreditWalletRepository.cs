namespace App.Modules.Wallets.Domain.Wallets.CreditWallets;

public interface ICreditWalletRepository
{
    Task AddAsync(CreditWallet wallet);

    Task<CreditWallet> GetByIdAsync(WalletId id);
}
