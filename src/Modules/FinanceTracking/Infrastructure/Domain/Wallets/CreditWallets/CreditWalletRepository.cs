using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CreditWallets;

internal class CreditWalletRepository(
    FinanceTrackingContext financeTrackingContext,
    IWalletHistoryRepository walletHistoryRepository) : ICreditWalletRepository, ICreditWalletReadRepository
{
    public async Task AddAsync(CreditWallet wallet)
    {
        var walletHistory = WalletHistory.From(wallet);

        await walletHistoryRepository.AddAsync(walletHistory);
        await financeTrackingContext.AddAsync(wallet);
    }

    public async Task SaveAsync(CreditWallet wallet)
    {
        var walletHistory = await walletHistoryRepository.GetByWalletIdAsync(wallet.Id);
        walletHistory.AddRange(wallet.DomainEvents);
    }

    public async Task<CreditWallet> GetByIdAsync(WalletId id)
    {
        var wallet = await financeTrackingContext.CreditWallets.SingleOrDefaultAsync(wallet => wallet.Id == id);

        if (wallet is null)
        {
            throw new NotFoundRepositoryException<CreditWallet>(id.Value);
        }

        var walletHistory = await walletHistoryRepository.GetByWalletIdAsync(id);
        wallet.Load(walletHistory.DomainEvents);

        return wallet;
    }

    public async Task<CreditWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId)
    {
        var wallet = await GetByIdAsync(id);

        if (wallet.UserId != userId)
        {
            throw new AccessDeniedException();
        }

        return wallet;
    }

    public async Task<int> GetAvailableBalanceAsync(WalletId id)
    {
        var wallet = await GetByIdAsync(id);

        return wallet.AvailableBalance;
    }
}
