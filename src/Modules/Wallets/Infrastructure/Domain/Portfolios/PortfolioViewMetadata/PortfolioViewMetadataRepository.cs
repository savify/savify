using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Wallets.Domain.Portfolios;
using App.Modules.Wallets.Domain.Portfolios.PortfolioViewMetadata;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Domain.Portfolios.PortfolioViewMetadata;

internal class PortfolioViewMetadataRepository : IPortfolioViewMetadataRepository
{
    private readonly WalletsContext _walletsContext;

    public PortfolioViewMetadataRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(Modules.Wallets.Domain.Portfolios.PortfolioViewMetadata.PortfolioViewMetadata viewMetadata)
    {
        await _walletsContext.AddAsync(viewMetadata);
    }

    public async Task<Modules.Wallets.Domain.Portfolios.PortfolioViewMetadata.PortfolioViewMetadata> GetByIdAsync(PortfolioId portfolioId)
    {
        var viewMetadata = await _walletsContext.PortfoliosViewMetadata.SingleOrDefaultAsync(x => x.PortfolioId == portfolioId);

        if (viewMetadata is null)
        {
            throw new NotFoundRepositoryException<Modules.Wallets.Domain.Portfolios.PortfolioViewMetadata.PortfolioViewMetadata>(portfolioId.Value);
        }

        return viewMetadata;
    }
}
