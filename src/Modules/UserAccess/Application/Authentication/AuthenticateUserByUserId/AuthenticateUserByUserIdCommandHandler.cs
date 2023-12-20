using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Application.Authentication.RefreshTokens;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.Configuration.Localization;
using Dapper;
using Microsoft.Extensions.Localization;

namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUserByUserId;

internal class AuthenticateUserByUserIdCommandHandler(
    ISqlConnectionFactory sqlConnectionFactory,
    IAuthenticationTokenGenerator tokenGenerator,
    IRefreshTokenRepository refreshTokenRepository,
    ILocalizerProvider localizerProvider)
    : ICommandHandler<AuthenticateUserByUserIdCommand, TokensResult>
{
    private readonly IStringLocalizer _localizer = localizerProvider.GetLocalizer();

    public async Task<TokensResult> Handle(AuthenticateUserByUserIdCommand command, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"SELECT id, name, email, password, is_active AS isActive FROM {DatabaseConfiguration.Schema.Name}.users WHERE id = @userId";

        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { command.UserId });

        if (user == null)
        {
            throw new AuthenticationException(_localizer["User does not exist"]);
        }

        if (!user.IsActive)
        {
            throw new AuthenticationException(_localizer["User is not active"]);
        }

        var accessToken = tokenGenerator.GenerateAccessToken(user.Id);
        var refreshToken = tokenGenerator.GenerateRefreshToken();

        await refreshTokenRepository.UpdateAsync(user.Id, refreshToken.Value, refreshToken.Expires);

        return new TokensResult(accessToken.Value, refreshToken.Value);
    }
}
