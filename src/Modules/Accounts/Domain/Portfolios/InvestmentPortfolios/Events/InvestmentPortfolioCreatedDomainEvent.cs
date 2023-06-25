using App.BuildingBlocks.Domain;
using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Investments;
using App.Modules.Accounts.Domain.Users;

namespace App.Modules.Accounts.Domain.Portfolios.InvestmentPortfolios.Events;
public class InvestmentPortfolioCreatedDomainEvent : DomainEventBase
{
    public PortfolioId PortfolioId { get; }

    public UserId UserId { get; }

    public Currency Currency { get; }

    public InvestmentPortfolioCreatedDomainEvent(PortfolioId portfolioId, UserId userId, Currency currency)
    {
        PortfolioId = portfolioId;
        UserId = userId;
        Currency = currency;
    }
}
