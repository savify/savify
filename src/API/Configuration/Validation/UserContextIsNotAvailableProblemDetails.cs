using App.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Validation;

public class UserContextIsNotAvailableProblemDetails : ProblemDetails
{
    public UserContextIsNotAvailableProblemDetails(UserContextIsNotAvailableException exception)
    {
        Title = "User is not authenticated";
        Detail = exception.Message;
        Status = StatusCodes.Status401Unauthorized;
    }
}
