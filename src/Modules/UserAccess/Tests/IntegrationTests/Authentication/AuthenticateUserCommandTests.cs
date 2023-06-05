using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Modules.UserAccess.Application.Authentication.AuthenticateUser;
using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Application.Authentication.RefreshTokens;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.IntegrationTests.Users;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.Modules.UserAccess.IntegrationTests.Authentication;

[TestFixture]
public class AuthenticateUserCommandTests : TestBase
{
    [Test]
    public async Task AuthenticateUserCommand_Test()
    {
        using var scope = WebApplicationFactory.Services.CreateScope();
        var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
        
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value
        ));

        var tokens = await UserAccessModule.ExecuteCommandAsync(
            new AuthenticateUserCommand(UserSampleData.Email, UserSampleData.PlainPassword));

        var decodedAccessToken = DecodeJwtToken(tokens.AccessToken);
        var refreshToken = await refreshTokenRepository.GetByUserIdAsync(userId);
        
        var userIdFromToken = decodedAccessToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        Assert.That(Guid.Parse(userIdFromToken), Is.EqualTo(userId));
        Assert.That(tokens.RefreshToken, Is.EqualTo(refreshToken?.Value));
    }
    
    [Test]
    public void AuthenticateUserCommand_ForNonExistingUser_WillFail()
    {
        Assert.That(async () =>
        {
            await UserAccessModule.ExecuteCommandAsync(
                new AuthenticateUserCommand(UserSampleData.Email, UserSampleData.PlainPassword));
        }, Throws.TypeOf<AuthenticationException>().With.Message.EqualTo("Incorrect email or password"));
    }
    
    [Test]
    public async Task AuthenticateUserCommand_WithIncorrectPassword_WillFail()
    {
        await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value
        ));
        
        Assert.That(async () =>
        {
            await UserAccessModule.ExecuteCommandAsync(new AuthenticateUserCommand(UserSampleData.Email, "incorrect"));
        }, Throws.TypeOf<AuthenticationException>().With.Message.EqualTo("Incorrect email or password"));
    }
    
    [Test]
    public async Task AuthenticateUserCommand_ForDeactivatedUser_WillFail()
    {
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value
        ));
        await DeactivateUser(userId);
        
        Assert.That(async () =>
        {
            await UserAccessModule.ExecuteCommandAsync(new AuthenticateUserCommand(UserSampleData.Email, UserSampleData.PlainPassword));
        }, Throws.TypeOf<AuthenticationException>().With.Message.EqualTo("User is not active"));
    }
    
    private JwtSecurityToken DecodeJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        return handler.ReadJwtToken(token);
    }

    // TODO: change to command in Application layer
    private async Task DeactivateUser(Guid userId)
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = "UPDATE user_access.users u SET is_active = false WHERE u.id = @userId";

        await sqlConnection.ExecuteScalarAsync(sql, new { userId });
    }
}
