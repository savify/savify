using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest;

public class PasswordResetRequestId : TypedIdValueBase
{
    public PasswordResetRequestId(Guid value) : base(value)
    {
    }
}
