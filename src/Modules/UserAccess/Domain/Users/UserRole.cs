using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.Users.Rules;

namespace App.Modules.UserAccess.Domain.Users;

public record UserRole(string Value)
{
    public static UserRole Admin => new(nameof(Admin));

    public static UserRole User => new(nameof(User));

    public static List<UserRole> AvailableRoles => new() { Admin, User };

    public static UserRole From(string value)
    {
        var role = new UserRole(value);

        BusinessRuleChecker.CheckRules(new UserRoleShouldBeOneOfAvailableRolesRule(role));

        return role;
    }
}
