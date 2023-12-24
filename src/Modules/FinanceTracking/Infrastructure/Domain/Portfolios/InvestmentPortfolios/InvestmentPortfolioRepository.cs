using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Portfolios;
using App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Portfolios.InvestmentPortfolios;

internal class InvestmentPortfolioRepository(FinanceTrackingContext financeTrackingContext) : IInvestmentPortfolioRepository
{
    public async Task AddAsync(InvestmentPortfolio portfolio)
    {
        await financeTrackingContext.AddAsync(portfolio);
    }

    public async Task<InvestmentPortfolio> GetByIdAsync(PortfolioId id)
    {
        var investmentPortfolio = await financeTrackingContext.InvestmentPortfolios.SingleOrDefaultAsync(x => x.Id == id);

        if (investmentPortfolio is null)
        {
            throw new NotFoundRepositoryException<InvestmentPortfolio>(id.Value);
        }

        return investmentPortfolio;
    }
}
