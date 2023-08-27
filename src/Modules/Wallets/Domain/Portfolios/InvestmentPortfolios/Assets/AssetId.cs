using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.Portfolios.InvestmentPortfolios.Assets;

internal class AssetId : TypedIdValueBase
{
    public AssetId(Guid value) : base(value)
    { }
}
