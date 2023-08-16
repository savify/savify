using App.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Validation;

public class InvalidCommandProblemDetails : ProblemDetails
{
    public Dictionary<string, List<string>> Errors { get; }

    public InvalidCommandProblemDetails(InvalidCommandException exception)
    {
        Title = "Command validation error";
        Status = StatusCodes.Status400BadRequest;
        Errors = exception.Errors;
    }
}
