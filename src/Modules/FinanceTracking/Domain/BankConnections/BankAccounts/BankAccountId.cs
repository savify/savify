using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;

public class BankAccountId : TypedIdValueBase
{
    public BankAccountId(Guid value) : base(value)
    {
    }
}
