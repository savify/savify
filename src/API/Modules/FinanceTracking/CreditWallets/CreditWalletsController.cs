using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.CreditWallets.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.EditCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.RemoveCreditWallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.CreditWallets;

[Authorize]
[ApiController]
[Route("wallets/credit-wallets")]
public class CreditWalletsController : ControllerBase
{
    private readonly IFinanceTrackingModule _financeTrackingModule;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public CreditWalletsController(IFinanceTrackingModule financeTrackingModule, IExecutionContextAccessor executionContextAccessor)
    {
        _financeTrackingModule = financeTrackingModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.AddNewWallet)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewCreditWalletRequest request)
    {
        var walletId = await _financeTrackingModule.ExecuteCommandAsync(new AddNewCreditWalletCommand(
            _executionContextAccessor.UserId,
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
    [HasPermission(FinanceTrackingPermissions.EditWallets)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid walletId, EditCreditWalletRequest request)
    {
        await _financeTrackingModule.ExecuteCommandAsync(new EditCreditWalletCommand(
            _executionContextAccessor.UserId,
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
    [HasPermission(FinanceTrackingPermissions.RemoveWallets)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid walletId)
    {
        await _financeTrackingModule.ExecuteCommandAsync(new RemoveCreditWalletCommand(_executionContextAccessor.UserId, walletId));

        return NoContent();
    }
}
