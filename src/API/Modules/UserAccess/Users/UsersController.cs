using App.API.Modules.UserAccess.Users.Requests;
using App.BuildingBlocks.Application;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Application.Users.GetUsers;
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
    
    [HttpPost("")]
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

    [HttpGet("")]
    public async Task<IActionResult> GetList()
    {
        var users = await _userAccessModule.ExecuteQueryAsync(new GetUsersQuery());

        return Ok(users);
    }
}
