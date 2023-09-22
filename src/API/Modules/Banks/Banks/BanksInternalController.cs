using App.API.Configuration.Authorization;
using App.API.Requests;
using App.Modules.Banks.Application.Banks.Internal;
using App.Modules.Banks.Application.Banks.Internal.GetBank;
using App.Modules.Banks.Application.Banks.Internal.GetBanks;
using App.Modules.Banks.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Banks.Banks;

[Authorize]
[ApiController]
[Route("banks/banks")]
public class BanksInternalController : ControllerBase
{
    private readonly IBanksModule _banksModule;

    public BanksInternalController(IBanksModule banksModule)
    {
        _banksModule = banksModule;
    }

    [HttpGet]
    [HasPermission(BanksPermissions.ManageBanks)]
    [ProducesResponseType(typeof(List<BankDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] PaginationQueryParameters pagination)
    {
        var banks = await _banksModule.ExecuteQueryAsync(
            new GetBanksQuery(pagination.Page, pagination.PerPage));

        return Ok(banks);
    }

    [HttpGet("{bankId}")]
    [HasPermission(BanksPermissions.ManageBanks)]
    [ProducesResponseType(typeof(BankDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid bankId)
    {
        var bank = await _banksModule.ExecuteQueryAsync(new GetBankQuery(bankId));

        return Ok(bank);
    }
}
