using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.IntegrationTests.Users;

[TestFixture]
public class UserDetailsProviderTests : TestBase
{
    [Test]
    public async Task TestThat_ProvidesUserId_ByUserEmail()
    {
        using var scope = WebApplicationFactory.Services.CreateScope();
        var userDetailsProvider = scope.ServiceProvider.GetRequiredService<IUserDetailsProvider>();

        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));

        var providedUserId = userDetailsProvider.ProvideUserIdByEmail(UserSampleData.Email);

        Assert.That(providedUserId.Value, Is.EqualTo(userId));
    }
}
