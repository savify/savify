using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Wallets.DebitWallets;

internal class DebitWalletRepository : IDebitWalletRepository
{
    private readonly FinanceTrackingContext _financeTrackingContext;

    public DebitWalletRepository(FinanceTrackingContext financeTrackingContext)
    {
        _financeTrackingContext = financeTrackingContext;
    }

    public async Task AddAsync(DebitWallet wallet)
    {
        await _financeTrackingContext.AddAsync(wallet);
    }

    public async Task<DebitWallet> GetByIdAsync(WalletId id)
    {
        var wallet = await _financeTrackingContext.DebitWallets.SingleOrDefaultAsync(wallet => wallet.Id == id);

        if (wallet is null)
        {
            throw new NotFoundRepositoryException<DebitWallet>(id.Value);
        }

        return wallet;
    }

    public async Task<DebitWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId)
    {
        var wallet = await _financeTrackingContext.DebitWallets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (wallet == null)
        {
            throw new NotFoundRepositoryException<DebitWallet>(
                "Wallet with id '{0}' was not found for user with id '{1}'",
                new object[] { id.Value, userId.Value });
        }

        return wallet;
    }
}
