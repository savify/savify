using App.API.Configuration.Authorization;
using App.API.Modules.UserAccess.Users.Requests;
using App.Modules.UserAccess.Application.Authorization;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Application.Users.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.UserAccess.Users;

[Route("user-access/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserAccessModule _userAccessModule;

    public UsersController(IUserAccessModule userAccessModule)
    {
        _userAccessModule = userAccessModule;
    }
    
    [Authorize]
    [HasPermission(UserAccessPermissions.ManageUsers)]
    [HttpPost("")]
    [ProducesResponseType( StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateNew(CreateNewUserRequest request)
    {
        var userId = await _userAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            request.Email,
            request.Password,
            request.Name,
            request.Role));
        
        return Created("",new
        {
            Id = userId
        });
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
