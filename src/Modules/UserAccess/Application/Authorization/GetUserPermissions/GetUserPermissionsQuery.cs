using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.Authorization.GetUserPermissions;

public class GetUserPermissionsQuery(Guid userId) : QueryBase<List<UserPermissionDto>>
{
    public Guid UserId { get; } = userId;
}
