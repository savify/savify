namespace App.Modules.Notifications.Domain.UserNotificationSettings;

public interface IUserNotificationSettingsRepository
{
    Task AddAsync(UserNotificationSettings userNotificationSettings);

    Task<UserNotificationSettings> GetByIdAsync(UserNotificationSettingsId id);

    Task<UserNotificationSettings> GetByUserEmailAsync(string email);
}
