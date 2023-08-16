using Microsoft.AspNetCore.Authorization;

namespace App.API.Configuration.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal class HasPermissionAttribute : AuthorizeAttribute
{
    internal const string HasPermissionPolicyName = "HasPermission";

    public string Name { get; }

    public HasPermissionAttribute(string name) : base(HasPermissionPolicyName)
    {
        Name = name;
    }
}
