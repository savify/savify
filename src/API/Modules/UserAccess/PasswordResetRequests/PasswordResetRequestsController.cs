using App.API.Configuration.Authorization;
using App.API.Modules.UserAccess.PasswordResetRequests.Requests;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Application.PasswordResetRequests.ConfirmPasswordReset;
using App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.UserAccess.PasswordResetRequests;

[Route("user-access/password-reset-requests")]
[ApiController]
public class PasswordResetRequestsController : ControllerBase
{
    private readonly IUserAccessModule _userAccessModule;

    public PasswordResetRequestsController(IUserAccessModule userAccessModule)
    {
        _userAccessModule = userAccessModule;
    }

    [AllowAnonymous]
    [NoPermissionRequired]
    [HttpPost("")]
    [ProducesResponseType( StatusCodes.Status201Created)]
    public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetRequest request)
    {
        var passwordResetRequestId = await _userAccessModule.ExecuteCommandAsync(
            new RequestPasswordResetCommand(request.Email));

        return Created("", new
        {
            Id = passwordResetRequestId
        });
    }
    
    [AllowAnonymous]
    [NoPermissionRequired]
    [HttpPatch("{passwordResetRequestId}/confirm")]
    [ProducesResponseType( StatusCodes.Status202Accepted)]
    public async Task<IActionResult> ConfirmPasswordReset(Guid passwordResetRequestId, ConfirmPasswordResetRequest request)
    {
        var token = await _userAccessModule.ExecuteCommandAsync(
            new ConfirmPasswordResetCommand(passwordResetRequestId, request.ConfirmationCode));

        return Accepted("", new
        {
            Token = token
        });
    }
}
