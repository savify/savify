using App.BuildingBlocks.Domain;

namespace App.Modules.Banks.Domain.Users;

public class UserId : TypedIdValueBase
{
    public UserId(Guid value) : base(value)
    {
    }
}
