using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletViewMetadata;

public class WalletViewMetadataRepository : IWalletViewMetadataRepository
{
    private readonly FinanceTrackingContext _financeTrackingContext;

    public WalletViewMetadataRepository(FinanceTrackingContext financeTrackingContext)
    {
        _financeTrackingContext = financeTrackingContext;
    }

    public async Task AddAsync(Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata.WalletViewMetadata walletViewMetadata)
    {
        await _financeTrackingContext.AddAsync(walletViewMetadata);
    }

    public async Task<Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata.WalletViewMetadata> GetByWalletIdAsync(WalletId walletId)
    {
        var walletViewMetadata = await _financeTrackingContext.WalletsViewMetadata.SingleOrDefaultAsync(x => x.WalletId == walletId);

        if (walletViewMetadata == null)
        {
            throw new NotFoundRepositoryException<Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata.WalletViewMetadata>(
                "View metadata for Wallet with id '{0}' was not found",
                new object[] { walletId.Value });
        }

        return walletViewMetadata;
    }
}
