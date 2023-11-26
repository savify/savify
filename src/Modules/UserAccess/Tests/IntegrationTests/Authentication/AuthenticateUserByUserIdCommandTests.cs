using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Modules.UserAccess.Application.Authentication.AuthenticateUserByUserId;
using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Application.Authentication.RefreshTokens;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.IntegrationTests.Users;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace App.Modules.UserAccess.IntegrationTests.Authentication;

[TestFixture]
public class AuthenticateUserByUserIdCommandTests : TestBase
{
    [Test]
    public async Task AuthenticateUserByUserIdCommandTest_Test()
    {
        using var scope = WebApplicationFactory.Services.CreateScope();
        var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();

        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));

        var tokens = await UserAccessModule.ExecuteCommandAsync(new AuthenticateUserByUserIdCommand(userId));

        var decodedAccessToken = DecodeJwtToken(tokens.AccessToken);
        var refreshToken = await refreshTokenRepository.GetByUserIdAsync(userId);

        var userIdFromToken = decodedAccessToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        Assert.That(Guid.Parse(userIdFromToken), Is.EqualTo(userId));
        Assert.That(tokens.RefreshToken, Is.EqualTo(refreshToken?.Value));
    }

    [Test]
    public void AuthenticateUserByUserIdCommand_ForNonExistingUser_WillFail()
    {
        Assert.That(async () => await UserAccessModule.ExecuteCommandAsync(new AuthenticateUserByUserIdCommand(Guid.NewGuid())),
            Throws.TypeOf<AuthenticationException>().With.Message.EqualTo("User does not exist"));
    }

    [Test]
    public async Task AuthenticateUserByUserIdCommand_ForDeactivatedUser_WillFail()
    {
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));
        await DeactivateUser(userId);

        Assert.That(async () => await UserAccessModule.ExecuteCommandAsync(new AuthenticateUserByUserIdCommand(userId)),
            Throws.TypeOf<AuthenticationException>().With.Message.EqualTo("User is not active"));
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

        var sql = $"UPDATE {DatabaseConfiguration.Schema.Name}.users u SET is_active = false WHERE u.id = @userId";

        await sqlConnection.ExecuteScalarAsync(sql, new { userId });
    }
}
