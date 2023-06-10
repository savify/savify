using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUserByUserId;

public class AuthenticateUserByUserIdCommand : CommandBase<TokensResult>
{
    public Guid UserId { get; }

    public AuthenticateUserByUserIdCommand(Guid userId)
    {
        UserId = userId;
    }
}
