using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Configuration.Localization;
using Microsoft.Extensions.Localization;

namespace App.Modules.UserAccess.Application.Authentication.RefreshTokens;

internal class RefreshTokensCommandHandler : ICommandHandler<RefreshTokensCommand, TokensResult>
{
    private readonly IStringLocalizer _localizer;

    private readonly IAuthenticationTokenGenerator _tokenGenerator;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokensCommandHandler(
        ILocalizerProvider localizerProvider,
        IAuthenticationTokenGenerator tokenGenerator,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _localizer = localizerProvider.GetLocalizer();
        _tokenGenerator = tokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<TokensResult> Handle(RefreshTokensCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByUserIdAsync(command.UserId);

        if (refreshToken == null || refreshToken.Value != command.RefreshToken)
        {
            throw new AuthenticationException(_localizer["Invalid refresh token"]);
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new AuthenticationException(_localizer["Refresh token expired"]);
        }

        var accessToken = _tokenGenerator.GenerateAccessToken(command.UserId);
        var newRefreshToken = _tokenGenerator.GenerateRefreshToken(refreshToken.Value);

        await _refreshTokenRepository.UpdateAsync(command.UserId, newRefreshToken.Value, newRefreshToken.Expires);

        return new TokensResult(accessToken.Value, refreshToken.Value);
    }
}
