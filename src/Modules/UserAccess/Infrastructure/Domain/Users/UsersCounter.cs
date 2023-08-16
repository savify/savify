using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Domain.Users;
using Dapper;

namespace App.Modules.UserAccess.Infrastructure.Domain.Users;

public class UsersCounter : IUsersCounter
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public UsersCounter(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public int CountUsersWithEmail(string email)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT COUNT(*) FROM user_access.users WHERE email = @email";

        return connection.QuerySingle<int>(sql, new { email });
    }
}
