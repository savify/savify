using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Accounts.Domain.Investments;
using App.Modules.Accounts.Domain.Investments.InvestmentPortfolios;
using App.Modules.Accounts.Domain.Portfolios.InvestmentPortfolios;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Accounts.Infrastructure.Domain.Portfolios.InvestmentPortfolios;
internal class InvestmentPortfolioRepository : IInvestmentPortfolioRepository
{
    private readonly AccountsContext _context;

    public InvestmentPortfolioRepository(AccountsContext context)
    {
        _context = context;
    }

    public async Task AddAsync(InvestmentPortfolio portfolio)
    {
        await _context.AddAsync(portfolio);
    }

    public async Task<InvestmentPortfolio> GetByIdAsync(PortfolioId id)
    {
        var portfolio = await _context.InvestmentPortfolio.SingleOrDefaultAsync(portfolio => portfolio.Id == id);

        if (portfolio is null)
        {
            throw new NotFoundRepositoryException<InvestmentPortfolio>(id.Value);
        }

        return portfolio;
    }
}
