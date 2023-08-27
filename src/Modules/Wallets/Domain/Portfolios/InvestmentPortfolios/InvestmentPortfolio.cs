using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios.Assets;
using App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios.Events;

namespace App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios;

public class InvestmentPortfolio : Entity, IAggregateRoot
{
    public PortfolioId Id { get; private set; }

    private string _title;

    private List<Asset> _assets;

    public static InvestmentPortfolio AddNew(string title) => new InvestmentPortfolio(title);

    private InvestmentPortfolio(string title)
    {
        Id = new PortfolioId(Guid.NewGuid());
        _title = title;
        _assets = new List<Asset>();

        AddDomainEvent(new InvestmentPortfolioAddedDomainEvent(Id));
    }

    private InvestmentPortfolio()
    { }
}
