namespace App.Modules.Wallets.Domain.Wallets.DebitWallets;

public interface IDebitWalletRepository
{
    Task AddAsync(DebitWallet wallet);

    Task<DebitWallet> GetByIdAsync(WalletId id);
}