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
public class RefreshTokensCommandTests : TestBase
{
    [Test]
    public async Task RefreshTokensCommand_Test()
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
        
        var tokens = await UserAccessModule.ExecuteCommandAsync(
            new AuthenticateUserCommand(UserSampleData.Email, UserSampleData.PlainPassword));
        var refreshToken = await refreshTokenRepository.GetByUserIdAsync(userId);

        var newTokens = await UserAccessModule.ExecuteCommandAsync(new RefreshTokensCommand(userId, tokens.RefreshToken));
        var newRefreshToken = await refreshTokenRepository.GetByUserIdAsync(userId);

        Assert.That(tokens.RefreshToken, Is.EqualTo(newTokens.RefreshToken));
        Assert.That(newRefreshToken.ExpiresAt, Is.GreaterThan(refreshToken.ExpiresAt));
    }
    
    [Test]
    public async Task RefreshTokensCommand_ForNonExistingRefreshToken_WillFail()
    {
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));

        Assert.That(async () => await UserAccessModule.ExecuteCommandAsync(new RefreshTokensCommand(userId, "token")), 
            Throws.TypeOf<AuthenticationException>().With.Message.EqualTo("Invalid refresh token"));
    }
    
    [Test]
    public async Task RefreshTokensCommand_ForInvalidRefreshToken_WillFail()
    {
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));
        
        await UserAccessModule.ExecuteCommandAsync(new AuthenticateUserCommand(UserSampleData.Email, UserSampleData.PlainPassword));

        Assert.That(async () => await UserAccessModule.ExecuteCommandAsync(new RefreshTokensCommand(userId, "invalid")), 
            Throws.TypeOf<AuthenticationException>().With.Message.EqualTo("Invalid refresh token"));
    }
    
    [Test]
    public async Task RefreshTokensCommand_ForExpiredRefreshToken_WillFail()
    {
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));
        
        var tokens = await UserAccessModule.ExecuteCommandAsync(new AuthenticateUserCommand(UserSampleData.Email, UserSampleData.PlainPassword));

        await ExpireRefreshTokenForUser(userId);
        
        Assert.That(async () => await UserAccessModule.ExecuteCommandAsync(new RefreshTokensCommand(userId, tokens.RefreshToken)), 
            Throws.TypeOf<AuthenticationException>().With.Message.EqualTo("Refresh token expired"));
    }

    private async Task ExpireRefreshTokenForUser(Guid userId)
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = "UPDATE user_access.refresh_tokens r SET expires_at = @expiresAt WHERE r.user_id = @userId";

        await sqlConnection.ExecuteScalarAsync(sql, new { expiresAt = DateTime.UtcNow.AddDays(-1), userId });
    }
}
