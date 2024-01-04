using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CashWallets;

public class CashWalletRepository(FinanceTrackingContext financeTrackingContext) : ICashWalletRepository
{
    public async Task AddAsync(CashWallet wallet)
    {
        await financeTrackingContext.AddAsync(wallet);
    }

    public async Task<CashWallet> GetByIdAsync(WalletId id)
    {
        var wallet = await financeTrackingContext.CashWallets.SingleOrDefaultAsync(x => x.Id == id);

        if (wallet == null)
        {
            throw new NotFoundRepositoryException<CashWallet>(id.Value);
        }

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
}
