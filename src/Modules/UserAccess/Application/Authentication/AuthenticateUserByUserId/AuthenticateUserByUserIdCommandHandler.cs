using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Application.Authentication.RefreshTokens;
using App.Modules.UserAccess.Application.Configuration.Commands;
using Dapper;
using Microsoft.Extensions.Localization;

namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUserByUserId;

internal class AuthenticateUserByUserIdCommandHandler : ICommandHandler<AuthenticateUserByUserIdCommand, TokensResult>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    private readonly IAuthenticationTokenGenerator _tokenGenerator;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    private readonly IStringLocalizer _localizer;

    public AuthenticateUserByUserIdCommandHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        IAuthenticationTokenGenerator tokenGenerator,
        IRefreshTokenRepository refreshTokenRepository,
        IStringLocalizer localizer)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _tokenGenerator = tokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _localizer = localizer;
    }

    public async Task<TokensResult> Handle(AuthenticateUserByUserIdCommand command, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT id, name, email, password, is_active AS isActive FROM user_access.users WHERE id = @userId";

        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { command.UserId });

        if (user == null)
        {
            throw new AuthenticationException(_localizer["User does not exist"]);
        }

        if (!user.IsActive)
        {
            throw new AuthenticationException(_localizer["User is not active"]);
        }

        var accessToken = _tokenGenerator.GenerateAccessToken(user.Id);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        await _refreshTokenRepository.UpdateAsync(user.Id, refreshToken.Value, refreshToken.Expires);

        return new TokensResult(accessToken.Value, refreshToken.Value); ;
    }
}
