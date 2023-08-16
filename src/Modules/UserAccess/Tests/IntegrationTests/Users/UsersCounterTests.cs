using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.IntegrationTests.Users;

[TestFixture]
public class UsersCounterTests : TestBase
{
    [Test]
    public async Task TestThat_ForExistingUser_CountIsOne()
    {
        using var scope = WebApplicationFactory.Services.CreateScope();
        var usersCounter = scope.ServiceProvider.GetRequiredService<IUsersCounter>();

        await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));

        var count = usersCounter.CountUsersWithEmail(UserSampleData.Email);
        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void TestThat_ForNonExistingUser_CountIsZero()
    {
        using var scope = WebApplicationFactory.Services.CreateScope();
        var usersCounter = scope.ServiceProvider.GetRequiredService<IUsersCounter>();

        var count = usersCounter.CountUsersWithEmail(UserSampleData.Email);
        Assert.That(count, Is.EqualTo(0));
    }
}
