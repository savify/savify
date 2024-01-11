using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Currencies.GetCurrencies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.Currencies;

[AllowAnonymous]
[ApiController]
[Route("finance-tracking/currencies")]
public class CurrenciesController(IFinanceTrackingModule financeTrackingModule) : ControllerBase
{
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var currencies = await financeTrackingModule.ExecuteQueryAsync(new GetCurrenciesQuery());

        return Ok(currencies);
    }
}
