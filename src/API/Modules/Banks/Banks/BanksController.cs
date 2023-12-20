using App.API.Configuration.Authorization;
using App.API.Requests;
using App.Modules.Banks.Application.Banks.Public;
using App.Modules.Banks.Application.Banks.Public.GetBank;
using App.Modules.Banks.Application.Banks.Public.GetBanks;
using App.Modules.Banks.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Banks.Banks;

[Authorize]
[ApiController]
[Route("public/banks/banks")]
public class BanksController(IBanksModule banksModule) : ControllerBase
{
    [HttpGet]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IEnumerable<BankDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] PaginationQueryParameters pagination)
    {
        var banks = await banksModule.ExecuteQueryAsync(
            new GetBanksQuery(pagination.Page, pagination.PerPage));

        return Ok(banks);
    }

    [HttpGet("{bankId}")]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(BankDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid bankId)
    {
        var bank = await banksModule.ExecuteQueryAsync(new GetBankQuery(bankId));

        return Ok(bank);
    }
}
