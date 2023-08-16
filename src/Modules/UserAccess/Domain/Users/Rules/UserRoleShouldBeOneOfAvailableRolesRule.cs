using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.Users.Rules;

public class UserRoleShouldBeOneOfAvailableRolesRule : IBusinessRule
{
    private readonly UserRole _role;

    public UserRoleShouldBeOneOfAvailableRolesRule(UserRole role)
    {
        _role = role;
    }

    public bool IsBroken() => !UserRole.AvailableRoles.Contains(_role);

    public string MessageTemplate => "Provided role '{0}' should be one of available roles: {1}";

    public object[] MessageArguments => new object[] { _role.Value, string.Join(", ", UserRole.AvailableRoles.Select(r => r.Value)) };
}
