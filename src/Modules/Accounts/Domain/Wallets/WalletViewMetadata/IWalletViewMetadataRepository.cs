namespace App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;

public interface IWalletViewMetadataRepository
{
    Task AddAsync(WalletViewMetadata walletViewMetadata);

    Task<WalletViewMetadata> GetByAccountIdAsync(WalletId walletId);
}
