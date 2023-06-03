using App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using App.Modules.UserAccess.Domain.UserRegistrations;

namespace App.Modules.UserAccess.IntegrationTests.UserRegistrations;

[TestFixture]
public class GetUserRegistrationTests : TestBase
{
    [Test]
    public async Task GetUserRegistrationQuery_WithNonExistingUserRegistration_Test()
    {
        var userRegistration = await UserAccessModule.ExecuteQueryAsync(new GetUserRegistrationQuery(Guid.NewGuid()));

        Assert.Null(userRegistration);
    }
    
    [Test]
    public async Task GetUserRegistrationQuery_Test()
    {
        var userRegistrationId = await UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            UserRegistrationSampleData.Email,
            UserRegistrationSampleData.PlainPassword,
            UserRegistrationSampleData.Name,
            UserRegistrationSampleData.PreferredLanguage.Value));

        var userRegistration = await UserAccessModule.ExecuteQueryAsync(new GetUserRegistrationQuery(userRegistrationId));

        Assert.That(userRegistration.Id, Is.EqualTo(userRegistrationId));
        Assert.That(userRegistration.Email, Is.EqualTo(UserRegistrationSampleData.Email));
        Assert.That(userRegistration.Name, Is.EqualTo(UserRegistrationSampleData.Name));
        Assert.That(userRegistration.Status, Is.EqualTo(UserRegistrationStatus.WaitingForConfirmation.Value));
        Assert.That(userRegistration.ValidTill, Is.GreaterThan(DateTime.UtcNow));
    }
}
