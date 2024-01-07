using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.WalletsHistory;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CashWallets;

public class CashWalletRepository(
    FinanceTrackingContext financeTrackingContext,
    IWalletHistoryRepository walletHistoryRepository) : ICashWalletRepository, ICashWalletReadRepository
{
    public async Task AddAsync(CashWallet wallet)
    {
        var walletHistory = WalletHistory.From(wallet);

        await walletHistoryRepository.AddAsync(walletHistory);
        await financeTrackingContext.AddAsync(wallet);
    }

    public async Task SaveAsync(CashWallet wallet)
    {
        var walletHistory = await walletHistoryRepository.GetByWalletIdAsync(wallet.Id);
        walletHistory.AddRange(wallet.DomainEvents);
    }

    public async Task<CashWallet> GetByIdAsync(WalletId id)
    {
        var wallet = await financeTrackingContext.CashWallets.SingleOrDefaultAsync(x => x.Id == id);

        if (wallet == null)
        {
            throw new NotFoundRepositoryException<CashWallet>(id.Value);
        }

        var walletHistory = await walletHistoryRepository.GetByWalletIdAsync(id);
        wallet.Load(walletHistory.DomainEvents);

        return wallet;
    }

    public async Task<CashWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId)
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
