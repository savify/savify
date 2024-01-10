using App.BuildingBlocks.Application;
using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Authentication.TokenInvalidation;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Application.Users.SetNewPassword;

internal class SetNewPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordResetRequestRepository passwordResetRequestRepository,
    IExecutionContextAccessor executionContextAccessor,
    IAccessTokenInvalidator accessTokenInvalidator) : ICommandHandler<SetNewPasswordCommand>
{
    public async Task Handle(SetNewPasswordCommand command, CancellationToken cancellationToken)
    {
        var userId = new UserId(command.UserId);
        var user = await userRepository.GetByIdAsync(userId);

        var passwordResetRequest = passwordResetRequestRepository.GetActiveByUserIdOrNullAsync(userId);
        if (passwordResetRequest is not null)
        {
            var accessToken = executionContextAccessor.AccessToken;
            await accessTokenInvalidator.InvalidateAsync(accessToken);

            passwordResetRequest.Finish();
        }

        var password = PasswordHasher.HashPassword(command.Password);
        user.SetNewPassword(password);
    }
}
