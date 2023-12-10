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
}
