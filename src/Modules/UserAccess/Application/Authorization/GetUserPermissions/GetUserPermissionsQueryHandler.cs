using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.UserAccess.Application.Authorization.GetUserPermissions;

internal class GetUserPermissionsQueryHandler : IQueryHandler<GetUserPermissionsQuery, List<UserPermissionDto>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUserPermissionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<List<UserPermissionDto>> Handle(GetUserPermissionsQuery query, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT permission_code AS code
                   FROM {DatabaseConfiguration.Schema.Name}.user_permissions_view v
                   WHERE v.user_id = @UserId
                   """;
        var permissions = await connection.QueryAsync<UserPermissionDto>(sql, new { query.UserId });

        return permissions.AsList();
    }
}
