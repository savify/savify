using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Wallets;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;

public class WalletHistoryRepository(FinanceTrackingContext context) : IWalletHistoryRepository
{
    public async Task AddAsync(WalletHistory walletHistory)
    {
        await context.AddAsync(walletHistory);
    }

    public async Task<WalletHistory> GetByWalletIdAsync(WalletId walletId)
    {
        var walletHistory = await context.WalletHistories
            .Include(x => x.Events)
            .SingleOrDefaultAsync(x => x.WalletId == walletId);

        if (walletHistory is null)
        {
            throw new NotFoundRepositoryException<WalletHistory>(walletId.Value);
        }

        return walletHistory;
    }
}
