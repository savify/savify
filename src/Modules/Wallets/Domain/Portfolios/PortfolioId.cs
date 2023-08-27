using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.Portfolios;

public class PortfolioId : TypedIdValueBase
{
    public PortfolioId(Guid value) : base(value)
    { }
}
