using App.Modules.UserAccess.Application.Authentication.Exceptions;
using App.Modules.UserAccess.Application.Configuration.Commands;
using Microsoft.Extensions.Localization;

namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUser;

public class AuthenticateUserCommandHandler : ICommandHandler<AuthenticateUserCommand, TokensResult>
{
    private readonly IAuthenticationClient _client;
    private readonly IStringLocalizer _localizer;

    public AuthenticateUserCommandHandler(IAuthenticationClient client, IStringLocalizer localizer)
    {
        _client = client;
        _localizer = localizer;
    }

    public async Task<TokensResult> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var response = await _client.RequestTokens(command.Email, command.Password);

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
