using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.Incomes.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Incomes.AddNewIncome;
using App.Modules.FinanceTracking.Application.Incomes.EditIncome;
using App.Modules.FinanceTracking.Application.Incomes.RemoveIncome;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.Incomes;

[Authorize]
[ApiController]
[Route("finance-tracking/incomes")]
public class IncomesController(
    IFinanceTrackingModule financeTrackingModule,
    IExecutionContextAccessor executionContextAccessor)
    : ControllerBase
{
    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.ManageIncomes)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewIncomeRequest request)
    {
        var incomeId = await financeTrackingModule.ExecuteCommandAsync(new AddNewIncomeCommand(
            executionContextAccessor.UserId,
            request.TargetWalletId,
            request.CategoryId,
            request.Amount,
            request.Currency,
            request.MadeOn,
            request.Comment,
            request.Tags));

        return Created("", new
        {
            Id = incomeId
        });
    }

    [HttpPut("{incomeId}")]
    [HasPermission(FinanceTrackingPermissions.ManageIncomes)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid incomeId, EditIncomeRequest request)
    {
        await financeTrackingModule.ExecuteCommandAsync(new EditIncomeCommand(
            incomeId,
            executionContextAccessor.UserId,
            request.TargetWalletId,
            request.CategoryId,
            request.Amount,
            request.Currency,
            request.MadeOn,
            request.Comment,
            request.Tags));

        return Accepted();
    }

    [HttpDelete("{incomeId}")]
    [HasPermission(FinanceTrackingPermissions.ManageIncomes)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid incomeId)
    {
        await financeTrackingModule.ExecuteCommandAsync(new RemoveIncomeCommand(incomeId, executionContextAccessor.UserId));

        return NoContent();
    }
}
