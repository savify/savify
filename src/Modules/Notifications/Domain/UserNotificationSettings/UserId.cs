using App.BuildingBlocks.Domain;

namespace App.Modules.Notifications.Domain.UserNotificationSettings;

public class UserId : TypedIdValueBase
{
    public UserId(Guid value) : base(value)
    {
    }
}
