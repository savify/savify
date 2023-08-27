using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Wallets.Domain.Portfolios;
using App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Domain.Portfolios.InvestmentPortfolios;

internal class InvestmentPortfolioRepository : IInvestmentPortfolioRepository
{
    private readonly WalletsContext _walletsContext;

    public InvestmentPortfolioRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(InvestmentPortfolio portfolio)
    {
        await _walletsContext.AddAsync(portfolio);
    }

    public async Task<InvestmentPortfolio> GetAsync(PortfolioId id)
    {
        var investmentPortfolio = await _walletsContext.InvestmentPortfolios.SingleOrDefaultAsync(x => x.Id == id);

        if (investmentPortfolio is null)
        {
            throw new NotFoundRepositoryException<InvestmentPortfolio>(id.Value);
        }

        return investmentPortfolio;
    }
}
