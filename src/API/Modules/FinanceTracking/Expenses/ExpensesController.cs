using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.Expenses.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Expenses.AddNewExpense;
using App.Modules.FinanceTracking.Application.Expenses.EditExpense;
using App.Modules.FinanceTracking.Application.Expenses.RemoveExpense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.Expenses;

[Authorize]
[ApiController]
[Route("finance-tracking/expenses")]
public class ExpensesController(
    IFinanceTrackingModule financeTrackingModule,
    IExecutionContextAccessor executionContextAccessor)
    : ControllerBase
{
    [HttpPost("")]
    [HasPermission(FinanceTrackingPermissions.ManageExpenses)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewExpenseRequest request)
    {
        var expenseId = await financeTrackingModule.ExecuteCommandAsync(new AddNewExpenseCommand(
            executionContextAccessor.UserId,
            request.SourceWalletId,
            request.CategoryId,
            request.Amount,
            request.MadeOn,
            request.Comment,
            request.Tags));

        return Created("", new
        {
            Id = expenseId
        });
    }

    [HttpPut("{expenseId}")]
    [HasPermission(FinanceTrackingPermissions.ManageExpenses)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid expenseId, EditExpenseRequest request)
    {
        await financeTrackingModule.ExecuteCommandAsync(new EditExpenseCommand(
            expenseId,
            executionContextAccessor.UserId,
            request.SourceWalletId,
            request.CategoryId,
            request.Amount,
            request.MadeOn,
            request.Comment,
            request.Tags));

        return Accepted();
    }

    [HttpDelete("{expenseId}")]
    [HasPermission(FinanceTrackingPermissions.ManageExpenses)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid expenseId)
    {
        await financeTrackingModule.ExecuteCommandAsync(new RemoveExpenseCommand(expenseId, executionContextAccessor.UserId));

        return NoContent();
    }
}
