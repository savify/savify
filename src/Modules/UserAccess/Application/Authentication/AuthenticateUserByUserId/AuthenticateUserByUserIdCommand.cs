using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUserByUserId;

public class AuthenticateUserByUserIdCommand(Guid userId) : CommandBase<TokensResult>
{
    [LogMasked]
    public Guid UserId { get; } = userId;
}
