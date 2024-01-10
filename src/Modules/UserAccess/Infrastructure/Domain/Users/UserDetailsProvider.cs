using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Domain.Users;
using Dapper;

namespace App.Modules.UserAccess.Infrastructure.Domain.Users;

public class UserDetailsProvider(ISqlConnectionFactory sqlConnectionFactory) : IUserDetailsProvider
{
    public UserId ProvideUserIdByEmail(string email)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"SELECT id FROM {DatabaseConfiguration.Schema.Name}.users u WHERE u.email = @Email";

        var userId = connection.QuerySingleOrDefault<Guid>(sql, new { email });

        return new UserId(userId);
    }
}
