using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Authentication.RefreshTokens;

public class RefreshTokensCommand : CommandBase<TokensResult>
{
    [LogMasked]
    public Guid UserId { get; }

    [LogMasked]
    public string RefreshToken { get; }

    public RefreshTokensCommand(Guid userId, string refreshToken)
    {
        UserId = userId;
        RefreshToken = refreshToken;
    }
}
