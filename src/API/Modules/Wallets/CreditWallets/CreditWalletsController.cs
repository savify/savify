using App.API.Configuration.Authorization;
using App.BuildingBlocks.Application;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Application.Wallets.CreditWallets.AddNewCreditWallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Wallets.CreditWallets;

[Authorize]
[ApiController]
[Route("wallets/credit-wallets")]
public class CreditWalletsController : ControllerBase
{
    private readonly IWalletsModule _walletsModule;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public CreditWalletsController(IWalletsModule walletsModule, IExecutionContextAccessor executionContextAccessor)
    {
        _walletsModule = walletsModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost("")]
    [HasPermission(WalletsPermissions.AddNewWallet)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewCreditWalletRequest request)
    {
        var walletId = await _walletsModule.ExecuteCommandAsync(new AddNewCreditWalletCommand(
            _executionContextAccessor.UserId,
            request.Title,
            request.Currency,
            request.AvailableBalance,
            request.CreditLimit));

        return Created("", new
        {
            Id = walletId
        });
    }
}
