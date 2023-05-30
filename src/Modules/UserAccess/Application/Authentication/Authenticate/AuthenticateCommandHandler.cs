using System.Security.Claims;
using Dapper;
using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.Authentication.Authenticate;

internal class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, AuthenticationResult>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public AuthenticateCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<AuthenticationResult> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT id, name, email, password, is_active AS isActive FROM user_access.users WHERE email = @email";

        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new
        {
            command.Email
        });
        
        // TODO: localize authentication errors
        if (user == null)
        {
            return new AuthenticationResult("Incorrect email or password");
        }
        
        if (!user.IsActive)
        {
            return new AuthenticationResult("User is not active");
        }
        
        if (!PasswordHasher.IsPasswordValid(user.Password, command.Password))
        {
            return new AuthenticationResult("Incorrect email or password");
        }
        
        user.Claims = new List<Claim>();
        user.Claims.Add(new Claim(CustomClaimTypes.Email, user.Email));
        user.Claims.Add(new Claim(CustomClaimTypes.Name, user.Name));

        return new AuthenticationResult(user);
    }
}
