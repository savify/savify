using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.BankConnections;

public class BankId : TypedIdValueBase
{
    public BankId(Guid value) : base(value)
    {
    }
}
