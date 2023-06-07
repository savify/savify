namespace App.Modules.Notifications.Domain.UserNotificationSettings;

public record Language(string Value)
{
    public static Language From(string value) => new(value);
}
