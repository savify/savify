using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.PasswordResetRequests.ConfirmPasswordReset;
using App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Application.Users.SetNewPassword;
using Dapper;
using Npgsql;

namespace App.Modules.UserAccess.IntegrationTests.Users;

[TestFixture]
public class SetNewPasswordTests : TestBase
{
    [Test]
    public async Task SetNewPassword_ChangesPassword()
    {
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));
        var oldHashedPassword = await GetHashedPassword(userId);

        await UserAccessModule.ExecuteCommandAsync(new SetNewPasswordCommand(userId, "New_password123!"));

        var newHashedPassword = await GetHashedPassword(userId);
        Assert.That(newHashedPassword, Is.Not.EqualTo(oldHashedPassword));
    }

    [Test]
    public async Task SetNewPassword_DuringPasswordReset_InvalidatesAccessToken()
    {
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));

        var passwordResetRequestId = await UserAccessModule.ExecuteCommandAsync(new RequestPasswordResetCommand(UserSampleData.Email));
        await UserAccessModule.ExecuteCommandAsync(new ConfirmPasswordResetCommand(passwordResetRequestId, await GetConfirmationCode()));

        await UserAccessModule.ExecuteCommandAsync(new SetNewPasswordCommand(userId, "New_password123!"));

        var invalidatedAccessTokens = await GetInvalidatedAccessTokens();

        Assert.That(invalidatedAccessTokens, Contains.Item("access_token"));
    }

    private async Task<string> GetHashedPassword(Guid userId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT password FROM {DatabaseConfiguration.Schema.Name}.users WHERE users.id = @userId";

        return await connection.QuerySingleAsync<string>(sql, new { userId });
    }

    private async Task<string[]> GetInvalidatedAccessTokens()
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT value FROM {DatabaseConfiguration.Schema.Name}.invalidated_access_tokens";

        var invalidatedAccessTokens = await connection.QueryAsync<string>(sql);

        return invalidatedAccessTokens.ToArray();
    }

    private async Task<string> GetConfirmationCode()
    {
        var notification = await GetLastOutboxMessage<PasswordResetRequestedNotification>();

        return notification.DomainEvent.ConfirmationCode.Value;
    }
}
