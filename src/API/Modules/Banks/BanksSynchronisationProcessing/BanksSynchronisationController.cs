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
public class BanksSynchronisationController(
    IBanksModule banksModule,
    IExecutionContextAccessor executionContextAccessor)
    : ControllerBase
{
    [HttpPost]
    [HasPermission(BanksPermissions.ManageBanks)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> SynchroniseBanks()
    {
        var result = await banksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(executionContextAccessor.UserId));

        return Created("", new
        {
            result.Status
        });
    }
}
