namespace App.Modules.Notifications.Domain.UserNotificationSettings;

public interface IUserNotificationSettingsCounter
{
    int CountNotificationSettingsWithUserId(UserId userId);
}
