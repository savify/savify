namespace App.Modules.Notifications.Application.UserNotificationSettings.GetUserNotificationSettings;

public class UserNotificationSettingsDto
{
    public required Guid UserId { get; set; }

    public required string Email { get; set; }

    public required string Name { get; set; }

    public required string PreferredLanguage { get; set; }
}
