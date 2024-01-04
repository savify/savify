using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.CreditWallets;

internal class CreditWalletRepository(FinanceTrackingContext financeTrackingContext) : ICreditWalletRepository
{
    public async Task AddAsync(CreditWallet wallet)
    {
        await financeTrackingContext.AddAsync(wallet);
    }

    public async Task<CreditWallet> GetByIdAsync(WalletId id)
    {
        var wallet = await financeTrackingContext.CreditWallets.SingleOrDefaultAsync(wallet => wallet.Id == id);

        if (wallet is null)
        {
            throw new NotFoundRepositoryException<CreditWallet>(id.Value);
        }

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
}
