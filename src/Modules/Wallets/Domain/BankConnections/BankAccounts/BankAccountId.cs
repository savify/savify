using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.BankConnections.BankAccounts;

public class BankAccountId : TypedIdValueBase
{
    public BankAccountId(Guid value) : base(value)
    {
    }
}
