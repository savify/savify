using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.UserAccess.Application.Users.GetUsers;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserDTO>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUsersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<List<UserDTO>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT id, name, email, is_active AS isActive, created_at AS createdAt, role_code AS role " +
                           "FROM users u " +
                           "INNER JOIN user_roles r ON u.id = r.user_id";
        var users = await connection.QueryAsync<UserDTO>(sql);

        return users.AsList();
    }
}
