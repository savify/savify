namespace App.Modules.Notifications.Domain.UserNotificationSettings;

public interface IUserNotificationSettingsRepository
{
    Task AddAsync(UserNotificationSettings userNotificationSettings);

    Task<UserNotificationSettings> GetByIdAsync(UserNotificationSettingsId id);
}
