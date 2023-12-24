using App.BuildingBlocks.Application.Exceptions;
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
            UserRegistrationSampleData.Country.Value,
            UserRegistrationSampleData.PreferredLanguage.Value));

        var userRegistration = await UserAccessModule.ExecuteQueryAsync(new GetUserRegistrationQuery(userRegistrationId));
        var notifications = await GetOutboxMessages();
        var notification = await GetLastOutboxMessage<NewUserRegisteredNotification>();

        Assert.That(notifications.Count, Is.EqualTo(1));
        Assert.That(notification.DomainEvent.UserRegistrationId.Value, Is.EqualTo(userRegistrationId));
        Assert.That(notification.DomainEvent.Email, Is.EqualTo(UserRegistrationSampleData.Email));
        Assert.That(notification.DomainEvent.Name, Is.EqualTo(UserRegistrationSampleData.Name));
        Assert.That(notification.DomainEvent.PreferredLanguage, Is.EqualTo(UserRegistrationSampleData.PreferredLanguage));

        Assert.That(userRegistration!.Id, Is.EqualTo(userRegistrationId));
        Assert.That(userRegistration.Email, Is.EqualTo(UserRegistrationSampleData.Email));
        Assert.That(userRegistration.Name, Is.EqualTo(UserRegistrationSampleData.Name));
        Assert.That(userRegistration.Status, Is.EqualTo(UserRegistrationStatus.WaitingForConfirmation.Value));
        Assert.That(userRegistration.ValidTill, Is.GreaterThan(DateTime.UtcNow));
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("invalid_email")]
    public void RegisterNewUserCommand_WhenEmailIsInvalid_ThrowsInvalidCommandException(string email)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            email,
            UserRegistrationSampleData.PlainPassword,
            UserRegistrationSampleData.Name,
            UserRegistrationSampleData.Country.Value,
            UserRegistrationSampleData.PreferredLanguage.Value)), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("min1!")]
    [TestCase("does_not_contain_uppercase_letter1!")]
    [TestCase("DOES_NOT_CONTAIN_LOWERCASE_LETTER1!")]
    [TestCase("Does_not_contain_numbers!")]
    [TestCase("Does_not_contain_special_characters1")]
    public void RegisterNewUserCommand_WhenPasswordIsInvalid_ThrowsInvalidCommandException(string password)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            UserRegistrationSampleData.Email,
            password,
            UserRegistrationSampleData.Name,
            UserRegistrationSampleData.Country.Value,
            UserRegistrationSampleData.PreferredLanguage.Value)), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    public void RegisterNewUserCommand_WhenNameIsInvalid_ThrowsInvalidCommandException(string name)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            UserRegistrationSampleData.Email,
            UserRegistrationSampleData.PlainPassword,
            name,
            UserRegistrationSampleData.Country.Value,
            UserRegistrationSampleData.PreferredLanguage.Value)), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("pl")]
    [TestCase("PLN")]
    public void RegisterNewUserCommand_WhenCountryIsInvalid_ThrowsInvalidCommandException(string country)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            UserRegistrationSampleData.Email,
            UserRegistrationSampleData.PlainPassword,
            UserRegistrationSampleData.Name,
            country,
            UserRegistrationSampleData.PreferredLanguage.Value)), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("PL")]
    [TestCase("PLN")]
    public void RegisterNewUserCommand_WhenPreferredLanguageIsInvalid_ThrowsInvalidCommandException(string preferredLanguage)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            UserRegistrationSampleData.Email,
            UserRegistrationSampleData.PlainPassword,
            UserRegistrationSampleData.Name,
            UserRegistrationSampleData.Country.Value,
            preferredLanguage)), Throws.TypeOf<InvalidCommandException>());
    }
}
