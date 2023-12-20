using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUser;

public class AuthenticateUserCommand(string email, string password) : CommandBase<TokensResult>
{
    [LogMasked]
    public string Email { get; } = email;

    [LogMasked]
    public string Password { get; } = password;
}
