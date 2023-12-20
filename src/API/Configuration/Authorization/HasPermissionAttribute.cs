using Microsoft.AspNetCore.Authorization;

namespace App.API.Configuration.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal class HasPermissionAttribute(string name) : AuthorizeAttribute(HasPermissionPolicyName)
{
    internal const string HasPermissionPolicyName = "HasPermission";

    public string Name { get; } = name;
}
