using App.BuildingBlocks.Domain;
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
            UserSampleData.Role.Value
        ));

        var providedUserId = userDetailsProvider.ProvideUserIdByEmail(UserSampleData.Email);
        
        Assert.That(providedUserId.Value, Is.EqualTo(userId));
    }
    
    [Test]
    public void TestThat_ProvideUserId_ByNonExistingEmail_ReturnNull()
    {
        using var scope = WebApplicationFactory.Services.CreateScope();
        var userDetailsProvider = scope.ServiceProvider.GetRequiredService<IUserDetailsProvider>();
        Assert.That(
            () => userDetailsProvider.ProvideUserIdByEmail(UserSampleData.Email), 
            Throws.TypeOf<DomainException>().With.Message
                .EqualTo($"User with email '{UserSampleData.Email}' does not exist"));
    }
}
