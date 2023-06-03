using App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using App.Modules.UserAccess.Domain.UserRegistrations;

namespace App.Modules.UserAccess.IntegrationTests.UserRegistrations;

[TestFixture]
public class RegisterNewUserTests : TestBase
{
    [Test]
    public async Task RegisterNewUserCommand_Test()
    {
        var userRegistrationId = await UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            UserRegistrationSampleData.Email,
            UserRegistrationSampleData.PlainPassword,
            UserRegistrationSampleData.Name,
            UserRegistrationSampleData.PreferredLanguage.Value));

        var userRegistration = await UserAccessModule.ExecuteQueryAsync(new GetUserRegistrationQuery(userRegistrationId));
        var notifications = await GetOutboxMessages();
        var notification = await GetLastOutboxMessage<NewUserRegisteredNotification>();
        
        Assert.That(notifications.Count, Is.EqualTo(1));
        Assert.That(notification.DomainEvent.UserRegistrationId.Value, Is.EqualTo(userRegistrationId));
        Assert.That(notification.DomainEvent.Email, Is.EqualTo(UserRegistrationSampleData.Email));
        Assert.That(notification.DomainEvent.Name, Is.EqualTo(UserRegistrationSampleData.Name));
        Assert.That(notification.DomainEvent.PreferredLanguage, Is.EqualTo(UserRegistrationSampleData.PreferredLanguage));
        
        Assert.That(userRegistration.Id, Is.EqualTo(userRegistrationId));
        Assert.That(userRegistration.Email, Is.EqualTo(UserRegistrationSampleData.Email));
        Assert.That(userRegistration.Name, Is.EqualTo(UserRegistrationSampleData.Name));
        Assert.That(userRegistration.Status, Is.EqualTo(UserRegistrationStatus.WaitingForConfirmation.Value));
        Assert.That(userRegistration.ValidTill, Is.GreaterThan(DateTime.UtcNow));
    }
}
