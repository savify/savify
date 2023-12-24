using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.ConfirmPasswordReset;

public class ConfirmPasswordResetCommand(Guid passwordResetRequestId, string confirmationCode) : CommandBase<string>
{
    public Guid PasswordResetRequestId { get; } = passwordResetRequestId;

    public string ConfirmationCode { get; } = confirmationCode;
}
