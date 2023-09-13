using App.API.Configuration.Authorization;
using App.Modules.Banks.Application.Banks.GetBanks;
using App.Modules.Banks.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Banks.Banks;

[Authorize]
[ApiController]
[Route("banks/banks")]
public class BanksController : ControllerBase
{
    private readonly IBanksModule _banksModule;

    public BanksController(IBanksModule banksModule)
    {
        _banksModule = banksModule;
    }

    [HttpGet]
    [HasPermission(BanksPermissions.ManageBanks)]
    [ProducesResponseType(typeof(List<BankDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList()
    {
        var banks = await _banksModule.ExecuteQueryAsync(new GetBanksQuery());

        return Ok(banks);
    }
}
