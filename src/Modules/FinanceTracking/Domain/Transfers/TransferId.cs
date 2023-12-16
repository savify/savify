using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Transfers;

public class TransferId : TypedIdValueBase
{
    public TransferId(Guid value) : base(value)
    {
    }
}
