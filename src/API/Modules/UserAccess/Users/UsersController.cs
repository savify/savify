using App.API.Configuration.Authorization;
using App.API.Modules.UserAccess.Users.Requests;
using App.BuildingBlocks.Application;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Application.Users.GetUsers;
using App.Modules.UserAccess.Application.Users.SetNewPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.UserAccess.Users;

[Route("user-access/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserAccessModule _userAccessModule;

    private readonly IExecutionContextAccessor _executionContextAccessor;

    public UsersController(IUserAccessModule userAccessModule, IExecutionContextAccessor executionContextAccessor)
    {
        _userAccessModule = userAccessModule;
        _executionContextAccessor = executionContextAccessor;
    }

    [Authorize]
    [HasPermission(UserAccessPermissions.ManageUsers)]
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateNew(CreateNewUserRequest request)
    {
        var userId = await _userAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            request.Email,
            request.Password,
            request.Name,
            request.Role,
            request.Country));

        return Created("", new
        {
            Id = userId
        });
    }

    [Authorize]
    [NoPermissionRequired]
    [HttpPatch("passwords")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> SetNewPassword(SetNewPasswordRequest request)
    {
        await _userAccessModule.ExecuteCommandAsync(new SetNewPasswordCommand(_executionContextAccessor.UserId, request.Password));

        return Accepted();
    }

    [Authorize]
    [HasPermission(UserAccessPermissions.ManageUsers)]
    [HttpGet("")]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList()
    {
        var users = await _userAccessModule.ExecuteQueryAsync(new GetUsersQuery());

        return Ok(users);
    }
}
