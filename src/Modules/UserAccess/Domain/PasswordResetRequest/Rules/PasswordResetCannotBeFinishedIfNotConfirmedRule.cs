using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Rules;

public class PasswordResetCannotBeFinishedIfNotConfirmedRule(PasswordResetRequestStatus status) : IBusinessRule
{
    public bool IsBroken() => status != PasswordResetRequestStatus.Confirmed;

    public string MessageTemplate => "Password reset cannot be finished if not confirmed";
}
