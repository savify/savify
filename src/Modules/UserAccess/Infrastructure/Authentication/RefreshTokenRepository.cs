using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Authentication;
using Dapper;

namespace App.Modules.UserAccess.Infrastructure.Authentication;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public RefreshTokenRepository(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<RefreshTokenDto?> GetByUserIdAsync(Guid userId)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        
        const string sql = "SELECT user_id as userId, value, expires_at as expiresAt FROM user_access.refresh_tokens WHERE user_id = @userId";
        
        var refreshToken = await connection.QuerySingleOrDefaultAsync<RefreshTokenDto>(sql, new { userId });

        return refreshToken;
    }

    public async Task UpdateAsync(Guid userId, string token, DateTime expiresAt)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        
        const string sql = "INSERT INTO user_access.refresh_tokens (user_id, value, expires_at) VALUES (@userId, @token, @expiresAt) " +
                           "ON CONFLICT (user_id) DO UPDATE SET value = @token, expires_at = @expiresAt";

        await connection.ExecuteAsync(sql, new {userId, token, expiresAt});
    }
}
