using App.BuildingBlocks.Application.Exceptions;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Application.Users.GetUsers;

namespace App.Modules.UserAccess.IntegrationTests.Users;

[TestFixture]
public class CreateNewUserTests : TestBase
{
    [Test]
    public async Task CreateNewUserCommand_Test()
    {
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
            ));

        var notifications = await GetOutboxMessages();
        var notification = await GetLastOutboxMessage<UserCreatedNotification>();
        var users = await UserAccessModule.ExecuteQueryAsync(new GetUsersQuery());
        var user = users.First(u => u.Id == userId);

        Assert.That(notifications.Count, Is.EqualTo(1));
        Assert.That(notification.DomainEvent.UserId.Value, Is.EqualTo(userId));
        Assert.That(user.Email, Is.EqualTo(UserSampleData.Email));
        Assert.That(user.Name, Is.EqualTo(UserSampleData.Name));
        Assert.That(user.IsActive, Is.True);
        Assert.That(user.Roles, Has.Member(UserSampleData.Role.Value));
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("invalid_email")]
    public void CreateNewUserCommand_WhenEmailIsInvalid_ThrowsInvalidCommandException(string email)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value)), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("min1!")]
    [TestCase("does_not_contain_uppercase_letter1!")]
    [TestCase("DOES_NOT_CONTAIN_LOWERCASE_LETTER1!")]
    [TestCase("Does_not_contain_numbers!")]
    [TestCase("Does_not_contain_special_characters1")]
    public void CreateNewUserCommand_WhenPasswordIsInvalid_ThrowsInvalidCommandException(string password)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            password,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value)), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("invalid_role")]
    public void CreateNewUserCommand_WhenRoleIsInvalid_ThrowsInvalidCommandException(string role)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            role,
            UserSampleData.Country.Value)), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("pl")]
    [TestCase("PLN")]
    public void CreateNewUserCommand_WhenCountryIsInvalid_ThrowsInvalidCommandException(string password)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            password,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value)), Throws.TypeOf<InvalidCommandException>());
    }
}
