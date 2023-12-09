namespace App.Modules.FinanceTracking.Domain.Portfolios.PortfolioViewMetadata;

public interface IPortfolioViewMetadataRepository
{
    Task AddAsync(PortfolioViewMetadata viewMetadata);
    Task<PortfolioViewMetadata> GetByIdAsync(PortfolioId portfolioId);
}
