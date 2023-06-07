using App.BuildingBlocks.Domain;

namespace App.Modules.Notifications.Domain.UserNotificationSettings;

public class UserNotificationSettingsId : TypedIdValueBase
{
    public UserNotificationSettingsId(Guid value) : base(value)
    {
    }
}
