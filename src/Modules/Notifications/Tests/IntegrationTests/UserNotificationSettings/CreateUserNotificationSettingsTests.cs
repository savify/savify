using App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Notifications.IntegrationTests.UserNotificationSettings;

[TestFixture]
public class CreateUserNotificationSettingsTests : TestBase
{
    [Test]
    public async Task CreateUserNotificationSettingsCommand_Test()
    {
        using var scope = WebApplicationFactory.Services.CreateScope();
        var userNotificationSettingsRepository = scope.ServiceProvider.GetRequiredService<IUserNotificationSettingsRepository>();
        var userId = Guid.NewGuid();

        await NotificationsModule.ExecuteCommandAsync(new CreateNotificationSettingsCommand(
            Guid.NewGuid(),
            userId,
            "Name",
            "test@email.com",
            "en"));

        var notificationSettings = await userNotificationSettingsRepository.GetByUserEmailAsync("test@email.com");

        Assert.That(notificationSettings.UserId.Value, Is.EqualTo(userId));
        Assert.That(notificationSettings.Name, Is.EqualTo("Name"));
        Assert.That(notificationSettings.Email, Is.EqualTo("test@email.com"));
        Assert.That(notificationSettings.PreferredLanguage, Is.EqualTo(Language.From("en")));
    }
}
