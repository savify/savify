using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios.Assets;

namespace App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios;

public class InvestmentPortfolio : Entity, IAggregateRoot
{
    public PortfolioId Id { get; private set; }

    private string _title;

    private List<Asset> _assets;

    public static InvestmentPortfolio AddNew(string title) => new InvestmentPortfolio(title);

    private InvestmentPortfolio(string title)
    {
        _title = title;

        Id = new PortfolioId(Guid.NewGuid());
        _assets = new List<Asset>();
    }

    private InvestmentPortfolio()
    { }
}
