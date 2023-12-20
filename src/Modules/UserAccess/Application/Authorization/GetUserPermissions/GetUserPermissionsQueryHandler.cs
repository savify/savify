using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.UserAccess.Application.Authorization.GetUserPermissions;

internal class GetUserPermissionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<GetUserPermissionsQuery, List<UserPermissionDto>>
{
    public async Task<List<UserPermissionDto>> Handle(GetUserPermissionsQuery query, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT permission_code AS code
                   FROM {DatabaseConfiguration.Schema.Name}.user_permissions_view v
                   WHERE v.user_id = @UserId
                   """;
        var permissions = await connection.QueryAsync<UserPermissionDto>(sql, new { query.UserId });

        return permissions.AsList();
    }
}
