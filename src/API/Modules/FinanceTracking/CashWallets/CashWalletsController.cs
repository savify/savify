using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.CashWallets.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.EditCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.RemoveCashWallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.CashWallets;

[Authorize]
[ApiController]
[Route("wallets/cash-wallets")]
public class CashWalletsController : ControllerBase
{
    private readonly IFinanceTrackingModule _financeTrackingModule;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public CashWalletsController(IFinanceTrackingModule financeTrackingModule, IExecutionContextAccessor executionContextAccessor)
    {
        _financeTrackingModule = financeTrackingModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.AddNewWallet)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewCashWalletRequest request)
    {
        var walletId = await _financeTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
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
    [HasPermission(FinanceTrackingPermissions.EditWallets)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid walletId, EditCashWalletRequest request)
    {
        await _financeTrackingModule.ExecuteCommandAsync(new EditCashWalletCommand(
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
    [HasPermission(FinanceTrackingPermissions.RemoveWallets)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid walletId)
    {
        await _financeTrackingModule.ExecuteCommandAsync(new RemoveCashWalletCommand(_executionContextAccessor.UserId, walletId));

        return NoContent();
    }
}
