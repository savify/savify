using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Portfolios.InvestmentPortfolios.Assets;

public class AssetId : TypedIdValueBase
{
    public AssetId(Guid value) : base(value)
    { }
}
