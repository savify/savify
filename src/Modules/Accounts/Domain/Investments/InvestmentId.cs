using App.BuildingBlocks.Domain;

namespace App.Modules.Accounts.Domain.Investments;
public class InvestmentId : TypedIdValueBase
{
    public InvestmentId(Guid value) : base(value)
    {
    }
}
