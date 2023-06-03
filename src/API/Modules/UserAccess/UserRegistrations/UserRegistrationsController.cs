using System.Globalization;
using App.API.Modules.UserAccess.UserRegistrations.Requests;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.UserAccess.UserRegistrations;

[Route("user-access/user-registrations")]
[ApiController]
public class UserRegistrationsController : ControllerBase
{
    private readonly IUserAccessModule _userAccessModule;

    public UserRegistrationsController(IUserAccessModule userAccessModule)
    {
        _userAccessModule = userAccessModule;
    }

    [AllowAnonymous]
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterNewUser(RegisterNewUserRequest request)
    {
        var userRegistrationId = await _userAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            request.Email,
            request.Password,
            request.Name,
            CultureInfo.CurrentCulture.Name));

        return Created("", new
        {
            Id = userRegistrationId
        });
    }

    [AllowAnonymous]
    [HttpGet("{userRegistrationId}")]
    [ProducesResponseType(typeof(UserRegistrationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserRegistration(Guid userRegistrationId)
    {
        var userRegistration =
            await _userAccessModule.ExecuteQueryAsync(new GetUserRegistrationQuery(userRegistrationId));

        return Ok(userRegistration);
    }
    
    [AllowAnonymous]
    [HttpPatch("{userRegistrationId}/confirm")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> ConfirmUserRegistration(Guid userRegistrationId, ConfirmUserRegistrationRequest request)
    {
        await _userAccessModule.ExecuteCommandAsync(new ConfirmUserRegistrationCommand(
            userRegistrationId,
            request.ConfirmationCode));

        return Accepted();
    }
    
    [AllowAnonymous]
    [HttpPatch("{userRegistrationId}/renew")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> RenewUserRegistration(Guid userRegistrationId)
    {
        await _userAccessModule.ExecuteCommandAsync(new RenewUserRegistrationCommand(userRegistrationId));

        return Accepted();
    }
}
