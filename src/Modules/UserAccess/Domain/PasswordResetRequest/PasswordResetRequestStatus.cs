namespace App.Modules.UserAccess.Domain.PasswordResetRequest;

public record PasswordResetRequestStatus(string Value)
{
    public static PasswordResetRequestStatus WaitingForConfirmation = new(nameof(WaitingForConfirmation));

    public static PasswordResetRequestStatus Confirmed = new(nameof(Confirmed));
}
