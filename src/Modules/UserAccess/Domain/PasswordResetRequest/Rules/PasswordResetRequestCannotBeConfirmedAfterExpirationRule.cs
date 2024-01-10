using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Rules;

public class PasswordResetRequestCannotBeConfirmedAfterExpirationRule(DateTime validTill) : IBusinessRule
{
    public bool IsBroken() => validTill < DateTime.UtcNow;

    public string MessageTemplate => "Password reset request cannot be confirmed because it was already expired";
}
