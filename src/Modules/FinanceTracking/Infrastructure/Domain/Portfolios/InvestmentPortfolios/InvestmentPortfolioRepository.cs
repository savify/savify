using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Portfolios;
using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Portfolios.InvestmentPortfolios;

internal class InvestmentPortfolioRepository : IInvestmentPortfolioRepository
{
    private readonly FinanceTrackingContext _financeTrackingContext;

    public InvestmentPortfolioRepository(FinanceTrackingContext financeTrackingContext)
    {
        _financeTrackingContext = financeTrackingContext;
    }

    public async Task AddAsync(InvestmentPortfolio portfolio)
    {
        await _financeTrackingContext.AddAsync(portfolio);
    }

    public async Task<InvestmentPortfolio> GetByIdAsync(PortfolioId id)
    {
        var investmentPortfolio = await _financeTrackingContext.InvestmentPortfolios.SingleOrDefaultAsync(x => x.Id == id);

        if (investmentPortfolio is null)
        {
            throw new NotFoundRepositoryException<InvestmentPortfolio>(id.Value);
        }

        return investmentPortfolio;
    }
}
