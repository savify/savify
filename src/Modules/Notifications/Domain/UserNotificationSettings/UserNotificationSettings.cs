using App.BuildingBlocks.Domain;
using App.Modules.Notifications.Domain.UserNotificationSettings.Rules;

namespace App.Modules.Notifications.Domain.UserNotificationSettings;

public class UserNotificationSettings : Entity, IAggregateRoot
{
    public UserNotificationSettingsId Id { get; private set; }
    
    public UserId UserId { get; private set; }
    
    public string Email { get; private set; }
    
    public string Name { get; private set; }
    
    public Language PreferredLanguage { get; private set; }

    public static UserNotificationSettings Create(
        UserId userId,
        string email,
        string name,
        Language preferredLanguage,
        IUserNotificationSettingsCounter userNotificationSettingsCounter)
    {
        CheckRules(new NotificationSettingsMustBeUniquePerUserIdRule(userId, userNotificationSettingsCounter));
        
        return new UserNotificationSettings(userId, email, name, preferredLanguage);
    }
    
    private UserNotificationSettings(UserId userId, string email, string name, Language preferredLanguage)
    {
        Id = new UserNotificationSettingsId(Guid.NewGuid());
        UserId = userId;
        Email = email;
        Name = name;
        PreferredLanguage = preferredLanguage;
    }

    private UserNotificationSettings() { }
}
