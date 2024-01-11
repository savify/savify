using App.BuildingBlocks.Application;
using App.Modules.UserAccess.Application.Authentication.RefreshTokens;
using App.Modules.UserAccess.Application.Authentication.TokenInvalidation;
using App.Modules.UserAccess.Application.Configuration.Commands;

namespace App.Modules.UserAccess.Application.Authentication.Logout;

internal class LogoutCommandHandler(
    IAccessTokenInvalidator accessTokenInvalidator,
    IExecutionContextAccessor executionContextAccessor,
    IRefreshTokenRepository refreshTokenRepository) : ICommandHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        await accessTokenInvalidator.InvalidateAsync(executionContextAccessor.AccessToken);
        await refreshTokenRepository.InvalidateAsync(executionContextAccessor.UserId);
    }
}
