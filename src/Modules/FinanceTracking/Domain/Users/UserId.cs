using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Users;

public class UserId : TypedIdValueBase
{
    public UserId(Guid value) : base(value)
    {
    }
}
