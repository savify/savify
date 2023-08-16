using App.BuildingBlocks.Application.Data;
using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.Users;
using Dapper;

namespace App.Modules.UserAccess.Infrastructure.Domain.Users;

public class UserDetailsProvider : IUserDetailsProvider
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public UserDetailsProvider(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public UserId ProvideUserIdByEmail(string email)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = "SELECT id FROM user_access.users u WHERE u.email = @Email";

        var userId = connection.QuerySingleOrDefault<Guid>(sql, new { email });

        if (userId.Equals(Guid.Empty))
        {
            throw new DomainException($"User with email '{email}' does not exist");
        }

        return new UserId(userId);
    }
}
