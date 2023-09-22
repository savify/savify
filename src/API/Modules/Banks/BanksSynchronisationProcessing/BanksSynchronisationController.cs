using App.API.Configuration.Authorization;
using App.BuildingBlocks.Application;
using App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;
using App.Modules.Banks.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Banks.BanksSynchronisationProcessing;

[Authorize]
[ApiController]
[Route("banks/sync")]
public class BanksSynchronisationController : ControllerBase
{
    private readonly IBanksModule _banksModule;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public BanksSynchronisationController(IBanksModule banksModule, IExecutionContextAccessor executionContextAccessor)
    {
        _banksModule = banksModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost]
    [HasPermission(BanksPermissions.ManageBanks)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> SynchroniseBanks()
    {
        var result = await _banksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(_executionContextAccessor.UserId));

        return Created("", new
        {
            result.Status
        });
    }
}
