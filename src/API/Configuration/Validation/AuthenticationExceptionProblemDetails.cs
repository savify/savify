using App.Modules.UserAccess.Application.Authentication.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Validation;

public class AuthenticationExceptionProblemDetails : ProblemDetails
{
    public AuthenticationExceptionProblemDetails(AuthenticationException exception)
    {
        Title = "User is not authenticated";
        Status = StatusCodes.Status401Unauthorized;
        Detail = exception.Message;
    }
}
