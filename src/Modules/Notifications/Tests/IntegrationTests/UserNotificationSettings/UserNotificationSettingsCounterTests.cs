using App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Notifications.IntegrationTests.UserNotificationSettings;

[TestFixture]
public class UserNotificationSettingsCounterTests : TestBase
{
    [Test]
    public async Task TestThat_ForExistingNotificationSettings_CountIsOne()
    {
        using var scope = WebApplicationFactory.Services.CreateScope();
        var userNotificationSettingsCounter = scope.ServiceProvider.GetRequiredService<IUserNotificationSettingsCounter>();
        var userId = Guid.NewGuid();
        
        await NotificationsModule.ExecuteCommandAsync(new CreateNotificationSettingsCommand(
            Guid.NewGuid(), 
            userId,
            "Name",
            "test@email.com",
            "en"));

        var count = userNotificationSettingsCounter.CountNotificationSettingsWithUserId(new UserId(userId));
        
        Assert.That(count, Is.EqualTo(1));
    }
    
    [Test]
    public async Task TestThat_ForNonExistingNotificationSettings_CountIsZero()
    {
        using var scope = WebApplicationFactory.Services.CreateScope();
        var userNotificationSettingsCounter = scope.ServiceProvider.GetRequiredService<IUserNotificationSettingsCounter>();

        var count = userNotificationSettingsCounter.CountNotificationSettingsWithUserId(new UserId(Guid.NewGuid()));
        
        Assert.That(count, Is.EqualTo(0));
    }
}
