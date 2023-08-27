namespace App.Modules.Wallets.Domain.Portfolios.PortfolioViewMetadata;

public interface IPortfolioViewMetadataRepository
{
    Task AddAsync(PortfolioViewMetadata viewMetadata);
    Task<PortfolioViewMetadata> GetAsync(PortfolioId portfolioId);
}
