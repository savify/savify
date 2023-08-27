namespace App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios;

public interface IInvestmentPortfolioRepository
{
    Task AddAsync(InvestmentPortfolio portfolio);
    Task<InvestmentPortfolio> GetAsync(PortfolioId id);
}
