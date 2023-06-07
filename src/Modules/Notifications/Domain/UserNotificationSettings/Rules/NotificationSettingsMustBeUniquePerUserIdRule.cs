using App.BuildingBlocks.Domain;

namespace App.Modules.Notifications.Domain.UserNotificationSettings.Rules;

public class NotificationSettingsMustBeUniquePerUserIdRule : IBusinessRule
{
    private readonly UserId _userId;

    private readonly IUserNotificationSettingsCounter _counter;

    public NotificationSettingsMustBeUniquePerUserIdRule(UserId userId, IUserNotificationSettingsCounter counter)
    {
        _userId = userId;
        _counter = counter;
    }

    public bool IsBroken() => _counter.CountNotificationSettingsWithUserId(_userId) > 0;

    public string MessageTemplate => "Notification settings for user with id '{0}' already exist";

    public object[] MessageArguments => new object[] { _userId.Value };
}
