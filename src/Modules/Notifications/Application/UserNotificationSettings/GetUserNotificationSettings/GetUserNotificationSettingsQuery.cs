using App.Modules.Notifications.Application.Contracts;

namespace App.Modules.Notifications.Application.UserNotificationSettings.GetUserNotificationSettings;

public class GetUserNotificationSettingsQuery(Guid userId) : QueryBase<UserNotificationSettingsDto?>
{
    public Guid UserId { get; } = userId;
}
