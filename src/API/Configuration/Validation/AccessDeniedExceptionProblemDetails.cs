using App.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Validation;

public class AccessDeniedExceptionProblemDetails : ProblemDetails
{
    public AccessDeniedExceptionProblemDetails(AccessDeniedException exception)
    {
        Title = "Access denied";
        Detail = exception.Message;
        Status = StatusCodes.Status403Forbidden;
    }
}
