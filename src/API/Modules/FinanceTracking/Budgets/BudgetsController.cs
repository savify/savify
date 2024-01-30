using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.Budgets.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Budgets.AddBudget;
using App.Modules.FinanceTracking.Application.Budgets.EditBudget;
using App.Modules.FinanceTracking.Application.Budgets.RemoveBudget;
using App.Modules.FinanceTracking.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.Budgets;

[Authorize]
[ApiController]
[Route("finance-tracking/budgets")]
public class BudgetsController(
    IFinanceTrackingModule financeTrackingModule,
    IExecutionContextAccessor executionContextAccessor) : ControllerBase
{
    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.ManageBudgets)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddBudgetRequest request)
    {
        var budgetId = await financeTrackingModule.ExecuteCommandAsync(new AddBudgetCommand(
            executionContextAccessor.UserId,
            request.StartDate,
            request.EndDate,
            request.CategoriesBudget,
            request.Currency
            ));

        return Created("", new
        {
            Id = budgetId
        });
    }

    [HttpPut("{budgetId}")]
    [HasPermission(FinanceTrackingPermissions.ManageBudgets)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Edit(Guid budgetId, EditBudgetRequest request)
    {
        await financeTrackingModule.ExecuteCommandAsync(new EditBudgetCommand(
            budgetId,
            executionContextAccessor.UserId,
            request.StartDate,
            request.EndDate,
            request.CategoriesBudget,
            request.Currency
        ));

        return Ok();
    }

    [HttpDelete("{budgetId}")]
    [HasPermission(FinanceTrackingPermissions.ManageBudgets)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove(Guid budgetId)
    {
        await financeTrackingModule.ExecuteCommandAsync(new RemoveBudgetCommand(
            budgetId,
            executionContextAccessor.UserId
        ));

        return NoContent();
    }
}
