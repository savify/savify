using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Authentication.TokenInvalidation;
using App.Modules.UserAccess.Application.Configuration.Data;
using Dapper;

namespace App.Modules.UserAccess.Infrastructure.Authentication.TokenInvalidation;

public class InvalidatedAccessTokenRepository(ISqlConnectionFactory sqlConnectionFactory) : IInvalidatedAccessTokenRepository
{
    public async Task AddAsync(string token, DateTime invalidatedAt)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   INSERT INTO {DatabaseConfiguration.Schema.Name}.invalidated_access_tokens (value, invalidated_at)
                   VALUES (@token, @invalidatedAt)
                   """;

        await connection.ExecuteAsync(sql, new { token, invalidatedAt });
    }

    public async Task<bool> IsInvalidatedAsync(string value)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"SELECT EXISTS(SELECT 1 FROM {DatabaseConfiguration.Schema.Name}.invalidated_access_tokens WHERE value = @value)";

        return await connection.QuerySingleOrDefaultAsync<bool>(sql, new { value });
    }
}
