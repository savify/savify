using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Application.Configuration.Commands;
using Microsoft.Extensions.Localization;

namespace App.Modules.UserAccess.Application.Authentication.RefreshTokens;

internal class RefreshTokensCommandHandler : ICommandHandler<RefreshTokensCommand, TokensResult>
{
    private readonly IAuthenticationClient _client;
    private readonly IStringLocalizer _localizer;

    public RefreshTokensCommandHandler(IAuthenticationClient client, IStringLocalizer localizer)
    {
        _client = client;
        _localizer = localizer;
    }

    public async Task<TokensResult> Handle(RefreshTokensCommand command, CancellationToken cancellationToken)
    {
        var response = await _client.RefreshTokens(command.RefreshToken);

        if (response.IsError && response.Error == "invalid_grant")
        {
            throw new AuthenticationException(_localizer["Invalid refresh token"]);
        }
        
        if (response.IsError)
        {
            throw new AuthenticationException(_localizer[response.ErrorDescription]);
        }

        return new TokensResult(
            response.AccessToken,
            response.RefreshToken,
            response.ExpiresIn);
    }
}
