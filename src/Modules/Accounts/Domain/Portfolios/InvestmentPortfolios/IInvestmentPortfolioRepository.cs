using App.Modules.Accounts.Domain.Investments;
using App.Modules.Accounts.Domain.Investments.InvestmentPortfolios;

namespace App.Modules.Accounts.Domain.Portfolios.InvestmentPortfolios;
public interface IInvestmentPortfolioRepository
{
    Task AddAsync(InvestmentPortfolio portfolio);

    Task<InvestmentPortfolio> GetByIdAsync(PortfolioId id);
}
