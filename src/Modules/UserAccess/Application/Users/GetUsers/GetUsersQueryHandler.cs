using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.UserAccess.Application.Users.GetUsers;

internal class GetUsersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<GetUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                    SELECT id, name, email, is_active AS isActive, created_at AS createdAt, array_agg(role_code) AS roles
                    FROM {DatabaseConfiguration.Schema.Name}.users u
                        INNER JOIN {DatabaseConfiguration.Schema.Name}.user_roles ur on u.id = ur.user_id
                    GROUP BY id
                   """;
        var users = await connection.QueryAsync<UserDto>(sql);

        return users.AsList();
    }
}
