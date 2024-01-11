using App.API.Modules.UserAccess.Authentication.Requests;
using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Authentication.AuthenticateUser;
using App.Modules.UserAccess.Application.Authentication.Logout;
using App.Modules.UserAccess.Application.Authentication.RefreshTokens;
using App.Modules.UserAccess.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.UserAccess.Authentication;

[Route("user-access/authentication")]
[ApiController]
public class AuthenticationController(IUserAccessModule userAccessModule) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("")]
    [ProducesResponseType(typeof(TokensResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> AuthenticateUser(AuthenticateUserRequest request)
    {
        var tokens = await userAccessModule.ExecuteCommandAsync(new AuthenticateUserCommand(request.Email, request.Password));

        return Ok(tokens);
    }

    [AllowAnonymous]
    [HttpPost("token/refresh")]
    [ProducesResponseType(typeof(TokensResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshAccessToken(RefreshAccessTokenRequest request)
    {
        var tokens = await userAccessModule.ExecuteCommandAsync(new RefreshTokensCommand(request.UserId, request.RefreshToken));

        return Ok(tokens);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await userAccessModule.ExecuteCommandAsync(new LogoutCommand());

        return Ok();
    }
}
