using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.Authentication.RefreshTokens;

public class RefreshTokensCommand : CommandBase<TokensResult>
{
    [LogMasked]
    public string RefreshToken { get; }

    public RefreshTokensCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
