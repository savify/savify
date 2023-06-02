using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.UserRegistrations;

public class UserRegistrationId : TypedIdValueBase
{
    public UserRegistrationId(Guid value) : base(value)
    {
    }
}
