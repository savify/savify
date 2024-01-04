using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.Wallets.CreditWallets.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.EditCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.RemoveCreditWallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.Wallets.CreditWallets;

[Authorize]
[ApiController]
[Route("finance-tracking/credit-wallets")]
public class CreditWalletsController(
    IFinanceTrackingModule financeTrackingModule,
    IExecutionContextAccessor executionContextAccessor)
    : ControllerBase
{
    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.ManageWallets)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewCreditWalletRequest request)
    {
        var walletId = await financeTrackingModule.ExecuteCommandAsync(new AddNewCreditWalletCommand(
            executionContextAccessor.UserId,
            request.Title,
            request.Currency,
            request.AvailableBalance,
            request.CreditLimit,
            request.Color,
            request.Icon,
            request.ConsiderInTotalBalance));

        return Created("", new
        {
            Id = walletId
        });
    }

    [HttpPatch("{walletId}")]
    [HasPermission(FinanceTrackingPermissions.ManageWallets)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid walletId, EditCreditWalletRequest request)
    {
        await financeTrackingModule.ExecuteCommandAsync(new EditCreditWalletCommand(
            executionContextAccessor.UserId,
            walletId,
            request.Title,
            request.Currency,
            request.AvailableBalance,
            request.CreditLimit,
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
        await financeTrackingModule.ExecuteCommandAsync(new RemoveCreditWalletCommand(executionContextAccessor.UserId, walletId));

        return NoContent();
    }
}
