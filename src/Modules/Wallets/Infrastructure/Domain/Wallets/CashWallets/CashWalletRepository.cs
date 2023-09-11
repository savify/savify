using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.CashWallets;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Domain.Wallets.CashWallets;

public class CashWalletRepository : ICashWalletRepository
{
    private readonly WalletsContext _walletsContext;

    public CashWalletRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(CashWallet wallet)
    {
        await _walletsContext.AddAsync(wallet);
    }

    public async Task<CashWallet> GetByIdAsync(WalletId id)
    {
        var wallet = await _walletsContext.CashWallets.SingleOrDefaultAsync(x => x.Id == id);

        if (wallet == null)
        {
            throw new NotFoundRepositoryException<CashWallet>(id.Value);
        }

        return wallet;
    }

    public async Task<CashWallet> GetByIdAndUserIdAsync(WalletId id, UserId userId)
    {
        var wallet = await _walletsContext.CashWallets.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (wallet == null)
        {
            throw new NotFoundRepositoryException<CashWallet>(
                "Wallet with id '{0}' was not found for user with id '{1}'",
                new object[] { id.Value, userId.Value });
        }

        return wallet;
    }
}
