using App.API.Configuration.Authorization;
using App.BuildingBlocks.Application;
using App.Modules.Accounts.Application.Contracts;
using App.Modules.Accounts.Application.DebitAccounts.AddNewDebitAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Accounts.DebitAccounts;

[Authorize]
[ApiController]
[Route("accounts/debit-accounts")]
public class DebitAccountsController : ControllerBase
{
    private readonly IAccountsModule _accountsModule;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public DebitAccountsController(IAccountsModule accountsModule, IExecutionContextAccessor executionContextAccessor)
    {
        _accountsModule = accountsModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost("")]
    [HasPermission(AccountsPermissions.AddNewAccount)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewDebitAccountRequest request)
    {
        var accountId = await _accountsModule.ExecuteCommandAsync(new AddNewDebitAccountCommand(
            _executionContextAccessor.UserId,
            request.Title,
            request.Currency,
            request.Balance));

        return Created("", new
        {
            Id = accountId
        });
    }
}
