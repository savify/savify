using System.Security.Claims;
using App.BuildingBlocks.Application;
using App.BuildingBlocks.Application.Exceptions;
using Microsoft.Net.Http.Headers;

namespace App.API.Configuration.ExecutionContext;

public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    public Guid UserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User!;

            if (user.FindFirst(ClaimTypes.NameIdentifier)?.Value != null)
            {
                return Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            }

            throw new UserContextIsNotAvailableException("User is not authenticated");
        }
    }

    public string AccessToken
    {
        get
        {
            if (IsAvailable && httpContextAccessor.HttpContext!.Request.Headers.Keys.Any(
                    x => x == HeaderNames.Authorization))
            {
                var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();
                var token = authorizationHeader.Split(' ')[1];

                return token;
            }

            throw new UserContextIsNotAvailableException("User is not authenticated");
        }
    }

    public Guid CorrelationId
    {
        get
        {
            if (IsAvailable && httpContextAccessor.HttpContext!.Request.Headers.Keys.Any(
                    x => x == CorrelationMiddleware.CorrelationHeaderKey))
            {
                return Guid.Parse(
                    httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]!);
            }

            throw new ApplicationException("Http context and correlation id is not available");
        }
    }

    public bool IsAvailable => httpContextAccessor.HttpContext != null;
}
