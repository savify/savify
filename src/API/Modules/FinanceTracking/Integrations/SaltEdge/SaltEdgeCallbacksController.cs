using App.API.Configuration.Authorization;
using App.Modules.FinanceTracking.Application.BankConnectionProcessing.CreateBankConnection;
using App.Modules.FinanceTracking.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.FinanceTracking.Integrations.SaltEdge;

[AllowAnonymous]
[ApiController]
[Route("integrations/salt-edge")]
public class SaltEdgeCallbacksController : ControllerBase
{
    private readonly IFinanceTrackingModule _financeTrackingModule;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public SaltEdgeCallbacksController(IFinanceTrackingModule financeTrackingModule, IHttpContextAccessor httpContextAccessor)
    {
        _financeTrackingModule = financeTrackingModule;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("success")]
    [NoPermissionRequired]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Success(SaltEdgeCallbackRequest request)
    {
        // TODO: verify signature
        var signature = Signature.CreateFromHttpContext(_httpContextAccessor.HttpContext);

        // TODO: accept all stages and make it prettier
        if (request.Data.Stage == "finish")
        {
            var result = await _financeTrackingModule.ExecuteCommandAsync(new CreateBankConnectionCommand(
                request.Data.CustomFields.BankConnectionProcessId,
                request.Data.ConnectionId));

            if (result.IsError && result.Error == CreateBankConnectionError.ExternalProviderError)
            {
                return Problem(statusCode: StatusCodes.Status424FailedDependency);
            }
        }

        return Accepted();
    }
}
