using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUserByUserId;

public class AuthenticateUserByUserIdCommand : CommandBase<TokensResult>
{
    [LogMasked]
    public Guid UserId { get; }

    public AuthenticateUserByUserIdCommand(Guid userId)
    {
        UserId = userId;
    }
}
