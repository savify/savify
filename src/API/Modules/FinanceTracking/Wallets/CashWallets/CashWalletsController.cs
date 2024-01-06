using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.Wallets.CashWallets.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.EditCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.RemoveCashWallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.Wallets.CashWallets;

[Authorize]
[ApiController]
[Route("finance-tracking/cash-wallets")]
public class CashWalletsController(
    IFinanceTrackingModule financeTrackingModule,
    IExecutionContextAccessor executionContextAccessor)
    : ControllerBase
{
    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.ManageWallets)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewCashWalletRequest request)
    {
        var walletId = await financeTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            executionContextAccessor.UserId,
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
    [HasPermission(FinanceTrackingPermissions.ManageWallets)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid walletId, EditCashWalletRequest request)
    {
        await financeTrackingModule.ExecuteCommandAsync(new EditCashWalletCommand(
            executionContextAccessor.UserId,
            walletId,
            request.Title,
            request.Balance,
            request.Color,
            request.Icon,
            request.ConsiderInTotalBalance));

        return Accepted();
    }

    [HttpDelete("{walletId}")]
    [HasPermission(FinanceTrackingPermissions.ManageWallets)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid walletId)
    {
        await financeTrackingModule.ExecuteCommandAsync(new RemoveCashWalletCommand(executionContextAccessor.UserId, walletId));

        return NoContent();
    }
}
