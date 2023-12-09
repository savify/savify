namespace App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios;

public interface IInvestmentPortfolioRepository
{
    Task AddAsync(InvestmentPortfolio portfolio);
    Task<InvestmentPortfolio> GetByIdAsync(PortfolioId id);
}
