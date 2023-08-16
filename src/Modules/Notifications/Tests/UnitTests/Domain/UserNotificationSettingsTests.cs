using App.Modules.Notifications.Domain.UserNotificationSettings;
using App.Modules.Notifications.Domain.UserNotificationSettings.Rules;

namespace App.Modules.Notifications.UnitTests.Domain;

[TestFixture]
public class UserNotificationSettingsTests : UnitTestBase
{
    [Test]
    public void CreatingUserNotificationSettings_ForNewUser_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var userNotificationSettingsCounter = Substitute.For<IUserNotificationSettingsCounter>();
        userNotificationSettingsCounter.CountNotificationSettingsWithUserId(userId).Returns(0);

        var notificationSettings = UserNotificationSettings.Create(
            userId,
            "test@email.com",
            "Test",
            Language.From("en"),
            userNotificationSettingsCounter);

        Assert.That(notificationSettings.UserId, Is.EqualTo(userId));
    }

    [Test]
    public void CreatingUserNotificationSettings_ForUserWithExistingNotificationSettings_BreaksNotificationSettingsMustBeUniquePerUserIdRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var userNotificationSettingsCounter = Substitute.For<IUserNotificationSettingsCounter>();
        userNotificationSettingsCounter.CountNotificationSettingsWithUserId(userId).Returns(1);

        AssertBrokenRule<NotificationSettingsMustBeUniquePerUserIdRule>(() => UserNotificationSettings.Create(
            userId,
            "test@email.com",
            "Test",
            Language.From("en"),
            userNotificationSettingsCounter));
    }
}
