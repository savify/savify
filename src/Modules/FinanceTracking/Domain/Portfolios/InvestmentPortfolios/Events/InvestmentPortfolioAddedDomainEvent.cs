using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios.Events;
public class InvestmentPortfolioAddedDomainEvent : DomainEventBase
{
    public PortfolioId PortfolioId { get; }

    public InvestmentPortfolioAddedDomainEvent(PortfolioId portfolioId)
    {
        PortfolioId = portfolioId;
    }
}
