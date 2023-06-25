using App.BuildingBlocks.Domain;

namespace App.Modules.Accounts.Domain.Investments;
public class PortfolioId : TypedIdValueBase
{
    public PortfolioId(Guid value) : base(value)
    {
    }
}
