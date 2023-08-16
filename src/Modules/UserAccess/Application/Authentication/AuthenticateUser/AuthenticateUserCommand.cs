using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUser;

public class AuthenticateUserCommand : CommandBase<TokensResult>
{
    [LogMasked]
    public string Email { get; }

    [LogMasked]
    public string Password { get; }

    public AuthenticateUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
