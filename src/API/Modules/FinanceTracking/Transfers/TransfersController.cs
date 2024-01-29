using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.Transfers.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.EditTransfer;
using App.Modules.FinanceTracking.Application.Transfers.RemoveTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.Transfers;

[Authorize]
[ApiController]
[Route("finance-tracking/transfers")]
public class TransfersController(
    IFinanceTrackingModule financeTrackingModule,
    IExecutionContextAccessor executionContextAccessor)
    : ControllerBase
{
    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.ManageTransfers)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewTransferRequest request)
    {
        var transferId = await financeTrackingModule.ExecuteCommandAsync(new AddNewTransferCommand(
            executionContextAccessor.UserId,
            request.SourceWalletId,
            request.TargetWalletId,
            request.SourceAmount,
            request.TargetAmount,
            request.MadeOn,
            request.Comment,
            request.Tags));

        return Created("", new
        {
            Id = transferId
        });
    }

    [HttpPut("{transferId}")]
    [HasPermission(FinanceTrackingPermissions.ManageTransfers)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid transferId, EditTransferRequest request)
    {
        await financeTrackingModule.ExecuteCommandAsync(new EditTransferCommand(
            transferId,
            executionContextAccessor.UserId,
            request.SourceWalletId,
            request.TargetWalletId,
            request.SourceAmount,
            request.TargetAmount,
            request.MadeOn,
            request.Comment,
            request.Tags));

        return Accepted();
    }

    [HttpDelete("{transferId}")]
    [HasPermission(FinanceTrackingPermissions.ManageTransfers)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid transferId)
    {
        await financeTrackingModule.ExecuteCommandAsync(new RemoveTransferCommand(transferId, executionContextAccessor.UserId));

        return NoContent();
    }
}
