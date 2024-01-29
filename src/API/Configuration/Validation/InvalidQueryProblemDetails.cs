using App.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Configuration.Validation;

public class InvalidQueryProblemDetails : ProblemDetails
{
    // TODO: add error list when query validation will be introduced
    // (see TODO in App.Modules.FinanceTracking.Application.ExchangeRates.GetExchangeRate.GetExchangeRateQueryHandler.Handle)
    public InvalidQueryProblemDetails(InvalidQueryException exception)
    {
        Title = "Query validation error";
        Status = StatusCodes.Status400BadRequest;
    }
}
