using App.Modules.UserAccess.Application.Authentication.Logout;
using App.Modules.UserAccess.Application.Configuration.Data;
using Dapper;
using Npgsql;

namespace App.Modules.UserAccess.IntegrationTests.Authentication;

[TestFixture]
public class LogoutCommandTests : TestBase
{
    [Test]
    public async Task LogoutCommand_InvalidatesAccessToken()
    {
        await UserAccessModule.ExecuteCommandAsync(new LogoutCommand());

        var invalidatedAccessTokens = await GetInvalidatedAccessTokens();

        Assert.That(invalidatedAccessTokens, Contains.Item("access_token"));
    }

    [Test]
    public async Task LogoutCommand_DeletesRefreshToken()
    {
        await InsertRefreshToken();

        await UserAccessModule.ExecuteCommandAsync(new LogoutCommand());

        var refreshTokenExists = await RefreshTokenExists();
        Assert.That(refreshTokenExists, Is.False);
    }

    private async Task InsertRefreshToken()
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"INSERT INTO {DatabaseConfiguration.Schema.Name}.refresh_tokens (user_id, value, expires_at) VALUES (@UserId, @Value, @ExpiresAt)";

        await connection.ExecuteAsync(sql, new
        {
            ExecutionContextAccessor.UserId,
            Value = "refresh_token",
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        });
    }

    private async Task<bool> RefreshTokenExists()
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT EXISTS(SELECT 1 FROM {DatabaseConfiguration.Schema.Name}.refresh_tokens WHERE user_id = @UserId)";

        return await connection.QuerySingleAsync<bool>(sql, new { ExecutionContextAccessor.UserId });
    }

    private async Task<string[]> GetInvalidatedAccessTokens()
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT value FROM {DatabaseConfiguration.Schema.Name}.invalidated_access_tokens";

        var invalidatedAccessTokens = await connection.QueryAsync<string>(sql);

        return invalidatedAccessTokens.ToArray();
    }
}
