using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Portfolios;

public class PortfolioId : TypedIdValueBase
{
    public PortfolioId(Guid value) : base(value)
    { }
}
