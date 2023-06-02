using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.Authorization.GetUserPermissions;

public class GetUserPermissionsQuery : QueryBase<List<UserPermissionDto>>
{
    public Guid UserId { get; }
    
    public GetUserPermissionsQuery(Guid userId)
    {
        UserId = userId;
    }
}