using App.BuildingBlocks.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Validation;

public class RepositoryExceptionProblemDetails : ProblemDetails
{
    public RepositoryExceptionProblemDetails(RepositoryException exception)
    {
        Title = "Resource not found";
        Detail = exception.Message;

        if (exception.GetType().GetGenericTypeDefinition() == typeof(NotFoundRepositoryException<>))
        {
            Status = StatusCodes.Status404NotFound;
        }
    }
}
