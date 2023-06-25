using App.API.Configuration.Authorization;
using App.BuildingBlocks.Application;
using App.Modules.Accounts.Application.Accounts.CashAccounts.AddNewCashAccount;
using App.Modules.Accounts.Application.CashAccounts.AddNewCashAccount;
using App.Modules.Accounts.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Accounts.CashAccounts;

[Authorize]
[ApiController]
[Route("accounts/cash-accounts")]
public class CashAccountsController : ControllerBase
{
    private readonly IAccountsModule _accountsModule;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public CashAccountsController(IAccountsModule accountsModule, IExecutionContextAccessor executionContextAccessor)
    {
        _accountsModule = accountsModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost("")]
    [HasPermission(AccountsPermissions.AddNewCashAccount)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewCashAccountRequest request)
    {
        var accountId = await _accountsModule.ExecuteCommandAsync(new AddNewCashAccountCommand(
            _executionContextAccessor.UserId,
            request.Title,
            request.Currency,
            request.Balance));

        return Created("", new
        {
            Id = accountId,
        });
    }
}
