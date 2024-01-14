using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets;

public class WalletsRepository(FinanceTrackingContext financeTrackingContext, IWalletHistoryRepository walletHistoryRepository) : IWalletsRepository
{
    public bool ExistsForUserIdAndWalletId(UserId userId, WalletId walletId)
    {
        return financeTrackingContext.CashWallets.Any(x => x.UserId == userId && x.Id == walletId) ||
               financeTrackingContext.DebitWallets.Any(x => x.UserId == userId && x.Id == walletId) ||
               financeTrackingContext.CreditWallets.Any(x => x.UserId == userId && x.Id == walletId);
    }

    public async Task<Wallet> GetByWalletIdAsync(WalletId walletId)
    {
        var wallet = await GetWalletWithoutLoadedHistoryAsync(walletId);

        var walletHistory = await walletHistoryRepository.GetByWalletIdAsync(walletId);
        wallet.Load(walletHistory.DomainEvents);

        return wallet;
    }

    public async Task<Wallet> GetByWalletIdAndUserIdAsync(WalletId walletId, UserId userId)
    {
        var wallet = await GetByWalletIdAsync(walletId);

        if (wallet.UserId != userId)
        {
            throw new AccessDeniedException();
        }

        return wallet;
    }

    public async Task UpdateHistoryAsync(Wallet wallet)
    {
        var walletHistory = await walletHistoryRepository.GetByWalletIdAsync(wallet.Id);
        walletHistory.AddRange(wallet.DomainEvents);
    }

    private async Task<Wallet> GetWalletWithoutLoadedHistoryAsync(WalletId walletId)
    {
        Wallet? wallet = null;
        var cashWalletOrNull = await financeTrackingContext.CashWallets.SingleOrDefaultAsync(x => x.Id == walletId);
        var debitWalletOrNull = await financeTrackingContext.DebitWallets.SingleOrDefaultAsync(x => x.Id == walletId);
        var creditWalletOrNull = await financeTrackingContext.CreditWallets.SingleOrDefaultAsync(x => x.Id == walletId);

        if (cashWalletOrNull is not null)
        {
            wallet = cashWalletOrNull;
        }

        if (debitWalletOrNull is not null)
        {
            wallet = debitWalletOrNull;
        }

        if (creditWalletOrNull is not null)
        {
            wallet = creditWalletOrNull;
        }

        return wallet ?? throw new NotFoundRepositoryException<Wallet>(walletId.Value);
    }
}
