using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Application.Authentication.RefreshTokens;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.Configuration.Localization;
using Dapper;
using Microsoft.Extensions.Localization;

namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUser;

internal class AuthenticateUserCommandHandler(
    ISqlConnectionFactory sqlConnectionFactory,
    IAuthenticationTokenGenerator tokenGenerator,
    IRefreshTokenRepository refreshTokenRepository,
    ILocalizerProvider localizerProvider)
    : ICommandHandler<AuthenticateUserCommand, TokensResult>
{
    private readonly IStringLocalizer _localizer = localizerProvider.GetLocalizer();

    public async Task<TokensResult> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"SELECT id, name, email, password, is_active AS isActive FROM {DatabaseConfiguration.Schema.Name}.users WHERE email = @email";

        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { command.Email });

        if (user == null)
        {
            throw new AuthenticationException(_localizer["Incorrect email or password"]);
        }

        if (!user.IsActive)
        {
            throw new AuthenticationException(_localizer["User is not active"]);
        }

        if (!PasswordHasher.IsPasswordValid(user.Password, command.Password))
        {
            throw new AuthenticationException(_localizer["Incorrect email or password"]);
        }

        var accessToken = tokenGenerator.GenerateAccessToken(user.Id);
        var refreshToken = tokenGenerator.GenerateRefreshToken();

        await refreshTokenRepository.UpdateAsync(user.Id, refreshToken.Value, refreshToken.Expires);

        return new TokensResult(accessToken.Value, refreshToken.Value);
    }
}
