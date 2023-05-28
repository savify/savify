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

        const string sql = "SELECT id, name, email, is_active AS isActive, created_at AS createdAt, array_agg(role_code) AS roles " +
                           "FROM user_access.users u " +
                           "INNER JOIN user_access.user_roles ur on u.id = ur.user_id " +
                           "GROUP BY id";
        var users = await connection.QueryAsync<UserDTO>(sql);

        return users.AsList();
    }
}
