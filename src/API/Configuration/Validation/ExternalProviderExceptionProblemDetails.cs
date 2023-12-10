using App.Modules.FinanceTracking.Infrastructure.Integrations.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Validation;

public class ExternalProviderExceptionProblemDetails : ProblemDetails
{
    public ExternalProviderExceptionProblemDetails(ExternalProviderException exception)
    {
        Title = "External provider error";
        Detail = exception.Message;
        Status = StatusCodes.Status424FailedDependency;
    }
}
