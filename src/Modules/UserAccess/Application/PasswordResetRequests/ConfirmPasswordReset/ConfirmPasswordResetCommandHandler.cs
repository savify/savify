using App.Modules.UserAccess.Application.Authentication;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.PasswordResetRequest;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.ConfirmPasswordReset;

internal class ConfirmPasswordResetCommandHandler(
    IPasswordResetRequestRepository passwordResetRequestRepository,
    IAuthenticationTokenGenerator authenticationTokenGenerator)
    : ICommandHandler<ConfirmPasswordResetCommand, string>
{
    public async Task<string> Handle(ConfirmPasswordResetCommand command, CancellationToken cancellationToken)
    {
        var passwordResetRequest = await passwordResetRequestRepository.GetByIdAsync(
            new PasswordResetRequestId(command.PasswordResetRequestId));

        passwordResetRequest.Confirm(ConfirmationCode.From(command.ConfirmationCode));

        var token = authenticationTokenGenerator.GenerateAccessToken(
            passwordResetRequest.UserId.Value,
            AccessTokenType.PasswordReset);

        return token.Value;
    }
}
