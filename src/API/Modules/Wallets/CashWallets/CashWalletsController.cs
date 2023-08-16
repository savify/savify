using App.API.Configuration.Authorization;
using App.BuildingBlocks.Application;
using App.Modules.Wallets.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.Wallets.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Wallets.CashWallets;

[Authorize]
[ApiController]
[Route("wallets/cash-wallets")]
public class CashWalletsController : ControllerBase
{
    private readonly IWalletsModule _walletsModule;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public CashWalletsController(IWalletsModule walletsModule, IExecutionContextAccessor executionContextAccessor)
    {
        _walletsModule = walletsModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost("")]
    [HasPermission(WalletsPermissions.AddNewWallet)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewCashWalletRequest request)
    {
        var walletId = await _walletsModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            _executionContextAccessor.UserId,
            request.Title,
            request.Currency,
            request.Balance,
            request.Color,
            request.Icon,
            request.ConsiderInTotalBalance));

        return Created("", new
        {
            Id = walletId,
        });
    }
}
