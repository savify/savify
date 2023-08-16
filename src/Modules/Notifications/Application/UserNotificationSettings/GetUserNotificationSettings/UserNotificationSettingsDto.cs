namespace App.Modules.Notifications.Application.UserNotificationSettings.GetUserNotificationSettings;

public class UserNotificationSettingsDto
{
    public Guid UserId { get; }

    public string Email { get; }

    public string Name { get; }

    public string PreferredLanguage { get; }
}
