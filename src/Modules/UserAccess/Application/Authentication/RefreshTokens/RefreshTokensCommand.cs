using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Authentication.RefreshTokens;

public class RefreshTokensCommand(Guid userId, string refreshToken) : CommandBase<TokensResult>
{
    [LogMasked]
    public Guid UserId { get; } = userId;

    [LogMasked]
    public string RefreshToken { get; } = refreshToken;
}
