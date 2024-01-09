using App.BuildingBlocks.Application;
using App.Modules.UserAccess.Application.Authentication.TokenInvalidation;
using Microsoft.AspNetCore.Authorization;

namespace App.API.Configuration.Authorization;

internal class CheckTokenInvalidationMiddleware(RequestDelegate next)
{
    public async Task Invoke(
        HttpContext context,
        IExecutionContextAccessor executionContextAccessor,
        IInvalidatedAccessTokenRepository invalidatedAccessTokenRepository)
    {
        var hasAuthorizeAttribute = context.GetEndpoint()?.Metadata.Any(m => m is AuthorizeAttribute);

        if (hasAuthorizeAttribute == true)
        {
            var isInvalidated = await invalidatedAccessTokenRepository.IsInvalidatedAsync(executionContextAccessor.AccessToken);

            if (isInvalidated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await next.Invoke(context);
    }
}
