using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Domain.Users;
using Dapper;

namespace App.Modules.UserAccess.Infrastructure.Domain.Users;

public class UsersCounter(ISqlConnectionFactory sqlConnectionFactory) : IUsersCounter
{
    public int CountUsersWithEmail(string email)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"SELECT COUNT(*) FROM {DatabaseConfiguration.Schema.Name}.users WHERE email = @email";

        return connection.QuerySingle<int>(sql, new { email });
    }
}
