using App.API.Configuration.Authorization;
using App.Modules.Wallets.Application.BankConnectionProcessing.CreateBankConnection;
using App.Modules.Wallets.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Wallets.Integrations.SaltEdge;

[AllowAnonymous]
[ApiController]
[Route("integrations/salt-edge")]
public class SaltEdgeCallbacksController : ControllerBase
{
    private readonly IWalletsModule _walletsModule;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public SaltEdgeCallbacksController(IWalletsModule walletsModule, IHttpContextAccessor httpContextAccessor)
    {
        _walletsModule = walletsModule;
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
            await _walletsModule.ExecuteCommandAsync(new CreateBankConnectionCommand(
                request.Data.CustomFields.BankConnectionProcessId,
                request.Data.ConnectionId));
        }
        
        return Accepted();
    }
}
