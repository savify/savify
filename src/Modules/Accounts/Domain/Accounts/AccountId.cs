using App.BuildingBlocks.Domain;

namespace App.Modules.Accounts.Domain.Accounts;

public class AccountId : TypedIdValueBase
{
    public AccountId(Guid value) : base(value)
    {
    }
}
