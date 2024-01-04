using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.UserTags;

[Authorize]
[ApiController]
[Route("finance-tracking/user-tags")]
public class UserTagsController(
    IFinanceTrackingModule financeTrackingModule,
    IExecutionContextAccessor executionContextAccessor)
    : ControllerBase
{
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
    {
        var userId = executionContextAccessor.UserId;

        var userTags = await financeTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        if (userTags is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            UserTags = userTags.Values
        });
    }
}
