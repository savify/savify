namespace App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

public interface IWalletViewMetadataRepository
{
    Task AddAsync(WalletViewMetadata walletViewMetadata);

    Task<WalletViewMetadata> GetByWalletIdAsync(WalletId walletId);
}
