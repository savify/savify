using App.API.Configuration.Authorization;
using App.BuildingBlocks.Application;
using App.Modules.Accounts.Application.Contracts;
using App.Modules.Accounts.Application.CreditAccounts.AddNewCreditAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Accounts.CreditAccounts;

[Authorize]
[ApiController]
[Route("accounts/credit-accounts")]
public class CreditAccountsController : ControllerBase
{
    private readonly IAccountsModule _accountsModule;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public CreditAccountsController(IAccountsModule accountsModule, IExecutionContextAccessor executionContextAccessor)
    {
        _accountsModule = accountsModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost("")]
    [HasPermission(AccountsPermissions.AddNewCreditAccount)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewCreditAccountRequest request)
    {
        var accountId = await _accountsModule.ExecuteCommandAsync(new AddNewCreditAccountCommand(
            _executionContextAccessor.UserId,
            request.Title,
            request.Currency,
            request.AvailableBalance,
            request.CreditLimit));

        return Created("", new
        {
            Id = accountId
        });
    }
}

public class AddNewCreditAccountRequest
{
    public string Title { get; set; }
    public string Currency { get; set; }
    public int AvailableBalance { get; set; }
    public int CreditLimit { get; set; }
}