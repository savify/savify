using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Authentication.Authenticate;

public class AuthenticateCommand : CommandBase<AuthenticationResult>
{
    [LogMasked]
    public string Email { get; }
    
    [LogMasked]
    public string Password { get; }

    public AuthenticateCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
