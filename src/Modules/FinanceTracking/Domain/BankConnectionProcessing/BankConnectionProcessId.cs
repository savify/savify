using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing;

public class BankConnectionProcessId : TypedIdValueBase
{
    public BankConnectionProcessId(Guid value) : base(value)
    {
    }
}
