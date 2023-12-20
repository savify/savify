using App.BuildingBlocks.Application.Exceptions;
using App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using App.Modules.UserAccess.Domain.UserRegistrations;

namespace App.Modules.UserAccess.IntegrationTests.UserRegistrations;

[TestFixture]
public class ConfirmUserRegistrationTests : TestBase
{
    [Test]
    public async Task ConfirmUserRegistrationCommand_Test()
    {
        var userRegistrationId = await UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            UserRegistrationSampleData.Email,
            UserRegistrationSampleData.PlainPassword,
            UserRegistrationSampleData.Name,
            UserRegistrationSampleData.Country.Value,
            UserRegistrationSampleData.PreferredLanguage.Value));

        var newUserRegisteredNotification = await GetLastOutboxMessage<NewUserRegisteredNotification>();
        var confirmationCode = newUserRegisteredNotification.DomainEvent.ConfirmationCode;

        await UserAccessModule.ExecuteCommandAsync(new ConfirmUserRegistrationCommand(
            userRegistrationId,
            confirmationCode.Value));

        var userRegistration = await UserAccessModule.ExecuteQueryAsync(new GetUserRegistrationQuery(userRegistrationId));
        var userRegistrationConfirmedNotification = await GetLastOutboxMessage<UserRegistrationConfirmedNotification>();

        Assert.That(userRegistration!.Status, Is.EqualTo(UserRegistrationStatus.Confirmed.Value));
        Assert.That(userRegistrationConfirmedNotification.DomainEvent.UserRegistrationId.Value, Is.EqualTo(userRegistrationId));
        Assert.That(userRegistrationConfirmedNotification.DomainEvent.Email, Is.EqualTo(UserRegistrationSampleData.Email));
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    public void ConfirmUserRegistrationCommand_WhenConfirmationCodeIsInvalid_ThrowsInvalidCommandException(string confirmationCode)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new ConfirmUserRegistrationCommand(
            Guid.NewGuid(), confirmationCode)), Throws.TypeOf<InvalidCommandException>());
    }
}
