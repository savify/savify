using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.DebitWallets;

internal class DebitWalletRepository(
    FinanceTrackingContext financeTrackingContext,
    IWalletHistoryRepository walletHistoryRepository) : IDebitWalletRepository, IDebitWalletReadRepository
{
    public async Task AddAsync(DebitWallet wallet)
    {
        var walletHistory = WalletHistory.From(wallet);

        await walletHistoryRepository.AddAsync(walletHistory);
        await financeTrackingContext.AddAsync(wallet);
    }

    public async Task UpdateHistoryAsync(DebitWallet wallet)
    {
        var walletHistory = await walletHistoryRepository.GetByWalletIdAsync(wallet.Id);
        walletHistory.AddRange(wallet.DomainEvents);
    }

    public async Task<DebitWallet> GetByIdAsync(WalletId id)
    {
        var wallet = await financeTrackingContext.DebitWallets.SingleOrDefaultAsync(wallet => wallet.Id == id);

        if (wallet is null)
        {
            throw new NotFoundRepositoryException<DebitWallet>(id.Value);
        }

        var walletHistory = await walletHistoryRepository.GetByWalletIdAsync(id);
        wallet.Load(walletHistory.DomainEvents);

        return wallet;
    }

    public async Task<DebitWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId)
    {
        var wallet = await GetByIdAsync(id);

        if (wallet.UserId != userId)
        {
            throw new AccessDeniedException();
        }

        return wallet;
    }

    public async Task<int> GetBalanceAsync(WalletId id)
    {
        var wallet = await GetByIdAsync(id);

        return wallet.Balance;
    }
}
