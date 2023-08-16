using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.PasswordResetRequests.ConfirmPasswordReset;

public class ConfirmPasswordResetCommand : CommandBase<string>
{
    public Guid PasswordResetRequestId { get; }

    public string ConfirmationCode { get; }

    public ConfirmPasswordResetCommand(Guid passwordResetRequestId, string confirmationCode)
    {
        PasswordResetRequestId = passwordResetRequestId;
        ConfirmationCode = confirmationCode;
    }
}
