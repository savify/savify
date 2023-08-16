using App.API.Configuration.Authorization;
using App.BuildingBlocks.Application;
using App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.Wallets.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Wallets.DebitWallets;

[Authorize]
[ApiController]
[Route("wallets/debit-wallets")]
public class DebitWalletsController : ControllerBase
{
    private readonly IWalletsModule _walletsModule;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public DebitWalletsController(IWalletsModule walletsModule, IExecutionContextAccessor executionContextAccessor)
    {
        _walletsModule = walletsModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [HttpPost("")]
    [HasPermission(WalletsPermissions.AddNewWallet)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddNew(AddNewDebitWalletRequest request)
    {
        var walletId = await _walletsModule.ExecuteCommandAsync(new AddNewDebitWalletCommand(
            _executionContextAccessor.UserId,
            request.Title,
            request.Currency,
            request.Balance,
            request.Color,
            request.Icon,
            request.ConsiderInTotalBalance));

        return Created("", new
        {
            Id = walletId
        });
    }
}
