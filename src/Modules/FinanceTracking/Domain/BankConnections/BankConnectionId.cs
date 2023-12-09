using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.BankConnections;

public class BankConnectionId : TypedIdValueBase
{
    public BankConnectionId(Guid value) : base(value)
    {
    }
}
