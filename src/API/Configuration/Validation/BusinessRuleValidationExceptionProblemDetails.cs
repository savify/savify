using App.BuildingBlocks.Domain;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Validation;

public class BusinessRuleValidationExceptionProblemDetails : ProblemDetails
{
    public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
    {
        Title = "Business rule broken";
        Status = StatusCodes.Status409Conflict;
        Detail = exception.Message;
    }
}
