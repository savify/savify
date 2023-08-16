using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Notifications.Infrastructure.Domain.UserNotificationSettings;

public class UserNotificationSettingsRepository : IUserNotificationSettingsRepository
{
    private readonly NotificationsContext _notificationsContext;

    public UserNotificationSettingsRepository(NotificationsContext notificationsContext)
    {
        _notificationsContext = notificationsContext;
    }

    public async Task AddAsync(Notifications.Domain.UserNotificationSettings.UserNotificationSettings userNotificationSettings)
    {
        await _notificationsContext.AddAsync(userNotificationSettings);
    }

    public async Task<Notifications.Domain.UserNotificationSettings.UserNotificationSettings> GetByIdAsync(UserNotificationSettingsId id)
    {
        var userNotificationSettings = await _notificationsContext.UserNotificationSettings.FirstOrDefaultAsync(x => x.Id == id);

        if (userNotificationSettings == null)
        {
            throw new NotFoundRepositoryException<Notifications.Domain.UserNotificationSettings.UserNotificationSettings>(id.Value);
        }

        return userNotificationSettings;
    }

    public async Task<Notifications.Domain.UserNotificationSettings.UserNotificationSettings> GetByUserEmailAsync(string email)
    {
        var userNotificationSettings = await _notificationsContext.UserNotificationSettings.FirstOrDefaultAsync(x => x.Email == email);

        if (userNotificationSettings == null)
        {
            throw new NotFoundRepositoryException<Notifications.Domain.UserNotificationSettings.UserNotificationSettings>(
                "UserNotificationSettings for user with email '{0}' was not found",
                new object[] { email });
        }

        return userNotificationSettings;
    }
}
