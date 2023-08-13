using App.BuildingBlocks.Infrastructure.Exceptions;
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
        var wallet = await _walletsContext.CashWallets.FirstOrDefaultAsync(x => x.Id == id);

        if (wallet == null)
        {
            throw new NotFoundRepositoryException<CashWallet>(id.Value);
        }

        return wallet;
    }
}
