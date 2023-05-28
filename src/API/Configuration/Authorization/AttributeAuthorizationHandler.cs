using Microsoft.AspNetCore.Authorization;

namespace App.API.Configuration.Authorization;

public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute> : AuthorizationHandler<TRequirement>
    where TRequirement : IAuthorizationRequirement
    where TAttribute : Attribute
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
    {
        var attribute = (context.Resource as HttpContext)?.GetEndpoint()?.Metadata.GetMetadata<TAttribute>();

        return HandleRequirementAsync(context, requirement, attribute ?? throw new InvalidOperationException());
    }

    protected abstract Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TRequirement requirement,
        TAttribute attribute);
}
