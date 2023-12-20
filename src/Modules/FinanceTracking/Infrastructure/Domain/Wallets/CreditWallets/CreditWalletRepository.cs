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
        var wallet = await financeTrackingContext.CreditWallets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (wallet == null)
        {
            throw new NotFoundRepositoryException<CreditWallet>(
                "Wallet with id '{0}' was not found for user with id '{1}'",
                new object[] { id.Value, userId.Value });
        }

        return wallet;
    }
}
