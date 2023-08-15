using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.CreditWallets;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Domain.Wallets.CreditWallets;

internal class CreditWalletRepository : ICreditWalletRepository
{
    private readonly WalletsContext _walletsContext;

    public CreditWalletRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(CreditWallet wallet)
    {
        await _walletsContext.AddAsync(wallet);
    }

    public async Task<CreditWallet> GetByIdAsync(WalletId id)
    {
        var wallet = await _walletsContext.CreditWallets.FirstOrDefaultAsync(wallet => wallet.Id == id);

        if (wallet is null)
        {
            throw new NotFoundRepositoryException<CreditWallet>(id.Value);
        }

        return wallet;
    }
}
