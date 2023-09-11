using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Domain.Wallets.WalletViewMetadata;

public class WalletViewMetadataRepository : IWalletViewMetadataRepository
{
    private readonly WalletsContext _walletsContext;

    public WalletViewMetadataRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(Modules.Wallets.Domain.Wallets.WalletViewMetadata.WalletViewMetadata walletViewMetadata)
    {
        await _walletsContext.AddAsync(walletViewMetadata);
    }

    public async Task<Modules.Wallets.Domain.Wallets.WalletViewMetadata.WalletViewMetadata> GetByWalletIdAsync(WalletId walletId)
    {
        var walletViewMetadata = await _walletsContext.WalletsViewMetadata.SingleOrDefaultAsync(x => x.WalletId == walletId);

        if (walletViewMetadata == null)
        {
            throw new NotFoundRepositoryException<Modules.Wallets.Domain.Wallets.WalletViewMetadata.WalletViewMetadata>(
                "View metadata for Wallet with id '{0}' was not found",
                new object[] { walletId.Value });
        }

        return walletViewMetadata;
    }
}
