using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

namespace App.Modules.UserAccess.IntegrationTests.UserRegistrations;

[TestFixture]
public class RenewUserRegistrationTests : TestBase
{
    [Test]
    public async Task RenewUserRegistrationCommand_Test()
    {
        var userRegistrationId = await UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            UserRegistrationSampleData.Email,
            UserRegistrationSampleData.PlainPassword,
            UserRegistrationSampleData.Name,
            UserRegistrationSampleData.Country.Value,
            UserRegistrationSampleData.PreferredLanguage.Value));

        var newUserRegisteredNotification = await GetLastOutboxMessage<NewUserRegisteredNotification>();
        var confirmationCode = newUserRegisteredNotification.DomainEvent.ConfirmationCode;

        await UserAccessModule.ExecuteCommandAsync(new RenewUserRegistrationCommand(userRegistrationId));
        var userRegistrationRenewedNotification = await GetLastOutboxMessage<UserRegistrationRenewedNotification>();
        var renewedConfirmationCode = userRegistrationRenewedNotification.DomainEvent.ConfirmationCode;

        Assert.That(renewedConfirmationCode, Is.Not.EqualTo(confirmationCode));
    }
}
