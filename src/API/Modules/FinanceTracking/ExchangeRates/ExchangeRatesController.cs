using App.API.Modules.FinanceTracking.ExchangeRates.Requests;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.ExchangeRates.GetExchangeRate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.ExchangeRates;

[AllowAnonymous]
[ApiController]
[Route("finance-tracking/exchange-rates")]
public class ExchangeRatesController(IFinanceTrackingModule financeTrackingModule) : ControllerBase
{
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentExchangeRate([FromQuery] GetCurrentExchangeRateRequest request)
    {
        var rate = await financeTrackingModule.ExecuteQueryAsync(new GetExchangeRateQuery(request.From, request.To));

        return Ok(new
        {
            Rate = rate
        });
    }
}
