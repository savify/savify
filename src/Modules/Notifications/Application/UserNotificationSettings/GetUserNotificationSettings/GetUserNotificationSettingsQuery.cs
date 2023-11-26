using App.Modules.Notifications.Application.Contracts;

namespace App.Modules.Notifications.Application.UserNotificationSettings.GetUserNotificationSettings;

public class GetUserNotificationSettingsQuery : QueryBase<UserNotificationSettingsDto?>
{
    public Guid UserId { get; }

    public GetUserNotificationSettingsQuery(Guid userId)
    {
        UserId = userId;
    }
}
