using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.Wallets.DebitWallets.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Wallets;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.EditDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.RemoveDebitWallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.Wallets.DebitWallets;

[Authorize]
[ApiController]
[Route("finance-tracking/debit-wallets")]
public class DebitWalletsController(
    IFinanceTrackingModule financeTrackingModule,
    IExecutionContextAccessor executionContextAccessor)
    : ControllerBase
{
    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.ManageWallets)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewDebitWalletRequest request)
    {
        var walletId = await financeTrackingModule.ExecuteCommandAsync(new AddNewDebitWalletCommand(
            executionContextAccessor.UserId,
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
    [HasPermission(FinanceTrackingPermissions.ManageWallets)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid walletId, EditDebitWalletRequest request)
    {
        await financeTrackingModule.ExecuteCommandAsync(new EditDebitWalletCommand(
            executionContextAccessor.UserId,
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
    [HasPermission(FinanceTrackingPermissions.ManageWallets)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid walletId)
    {
        await financeTrackingModule.ExecuteCommandAsync(new RemoveDebitWalletCommand(executionContextAccessor.UserId, walletId));

        return NoContent();
    }

    [HttpPost("{walletId}/bank-connection")]
    [HasPermission(FinanceTrackingPermissions.ConnectBankAccountsToWallets)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> ConnectBankAccount(Guid walletId, ConnectBankAccountToDebitWalletRequest request)
    {
        var result = await financeTrackingModule.ExecuteCommandAsync(new ConnectBankAccountToDebitWalletCommand(
            executionContextAccessor.UserId,
            walletId,
            request.BankId));

        if (result.IsError && result.Error == BankConnectionProcessInitiationError.ExternalProviderError)
        {
            return Problem(statusCode: StatusCodes.Status424FailedDependency);
        }

        return Created("", result.Success);
    }
}
