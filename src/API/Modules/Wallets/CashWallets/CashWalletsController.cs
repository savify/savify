using App.API.Configuration.Authorization;
using App.API.Modules.Wallets.CashWallets.Requests;
using App.BuildingBlocks.Application;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.Wallets.Application.Wallets.CashWallets.EditCashWallet;
using App.Modules.Wallets.Application.Wallets.CashWallets.RemoveCashWallet;
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

    [HttpPatch("{walletId}")]
    [HasPermission(WalletsPermissions.EditWallets)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid walletId, EditCashWalletRequest request)
    {
        await _walletsModule.ExecuteCommandAsync(new EditCashWalletCommand(
            _executionContextAccessor.UserId,
            walletId,
            request.Title,
            request.Currency,
            request.Balance,
            request.Color,
            request.Icon,
            request.ConsiderInTotalBalance));

        return Accepted();
    }

    [HttpDelete("{walletId}")]
    [HasPermission(WalletsPermissions.RemoveWallets)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid walletId)
    {
        await _walletsModule.ExecuteCommandAsync(new RemoveCashWalletCommand(_executionContextAccessor.UserId, walletId));

        return NoContent();
    }
}
