using App.API.Configuration.Authorization;
using App.API.Modules.FinanceTracking.BankConnectionProcessing.Requests;
using App.BuildingBlocks.Application;
using App.Modules.FinanceTracking.Application.BankConnectionProcessing.ChooseBankAccountToConnect;
using App.Modules.FinanceTracking.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.BankConnectionProcessing;

[Authorize]
[ApiController]
[Route("wallets/bank-connection-processing")]
public class BankConnectionProcessingController : ControllerBase
{
    private readonly IFinanceTrackingModule _financeTrackingModule;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public BankConnectionProcessingController(IFinanceTrackingModule financeTrackingModule, IExecutionContextAccessor executionContextAccessor)
    {
        _financeTrackingModule = financeTrackingModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPut("{bankConnectionProcessId}/choose-account")]
    [HasPermission(FinanceTrackingPermissions.ConnectBankAccountsToWallets)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> ChooseBankAccount(Guid bankConnectionProcessId, ChooseBankAccountRequest request)
    {
        await _financeTrackingModule.ExecuteCommandAsync(new ChooseBankAccountToConnectCommand(
            bankConnectionProcessId,
            _executionContextAccessor.UserId,
            request.BankAccountId));

        return Accepted();
    }
}
