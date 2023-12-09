using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletViewMetadata;

public class WalletViewMetadataRepository : IWalletViewMetadataRepository
{
    private readonly WalletsContext _walletsContext;

    public WalletViewMetadataRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata.WalletViewMetadata walletViewMetadata)
    {
        await _walletsContext.AddAsync(walletViewMetadata);
    }

    public async Task<Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata.WalletViewMetadata> GetByWalletIdAsync(WalletId walletId)
    {
        var walletViewMetadata = await _walletsContext.WalletsViewMetadata.SingleOrDefaultAsync(x => x.WalletId == walletId);

        if (walletViewMetadata == null)
        {
            throw new NotFoundRepositoryException<Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata.WalletViewMetadata>(
                "View metadata for Wallet with id '{0}' was not found",
                new object[] { walletId.Value });
        }

        return walletViewMetadata;
    }
}
