using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.DebitWallets.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Wallets;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.EditDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.RemoveDebitWallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.DebitWallets;

[Authorize]
[ApiController]
[Route("finance-tracking/debit-wallets")]
public class DebitWalletsController : ControllerBase
{
    private readonly IFinanceTrackingModule _financeTrackingModule;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public DebitWalletsController(IFinanceTrackingModule financeTrackingModule, IExecutionContextAccessor executionContextAccessor)
    {
        _financeTrackingModule = financeTrackingModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.AddNewWallet)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewDebitWalletRequest request)
    {
        var walletId = await _financeTrackingModule.ExecuteCommandAsync(new AddNewDebitWalletCommand(
            _executionContextAccessor.UserId,
            request.Title,
            request.Currency,
            request.Balance,
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
    public async Task<IActionResult> Edit(Guid walletId, EditDebitWalletRequest request)
    {
        await _financeTrackingModule.ExecuteCommandAsync(new EditDebitWalletCommand(
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
        await _financeTrackingModule.ExecuteCommandAsync(new RemoveDebitWalletCommand(_executionContextAccessor.UserId, walletId));

        return NoContent();
    }

    [HttpPost("{walletId}/bank-connection")]
    [HasPermission(FinanceTrackingPermissions.ConnectBankAccountsToWallets)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> ConnectBankAccount(Guid walletId, ConnectBankAccountToDebitWalletRequest request)
    {
        var result = await _financeTrackingModule.ExecuteCommandAsync(new ConnectBankAccountToDebitWalletCommand(
            _executionContextAccessor.UserId,
            walletId,
            request.BankId));

        if (result.IsError && result.Error == BankConnectionProcessInitiationError.ExternalProviderError)
        {
            return Problem(statusCode: StatusCodes.Status424FailedDependency);
        }

        return Created("", result.Success);
    }
}
