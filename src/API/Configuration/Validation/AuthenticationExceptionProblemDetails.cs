using App.Modules.UserAccess.Application.Authentication.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Validation;

public class AuthenticationExceptionProblemDetails : ProblemDetails
{
    public AuthenticationExceptionProblemDetails(AuthenticationException exception)
    {
        Title = "Authentication exception";
        Status = StatusCodes.Status401Unauthorized;
        Detail = exception.Message;
    }
}
