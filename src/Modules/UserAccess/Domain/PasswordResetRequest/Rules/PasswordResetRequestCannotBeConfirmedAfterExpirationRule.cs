using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Rules;

public class PasswordResetRequestCannotBeConfirmedAfterExpirationRule : IBusinessRule
{
    private readonly DateTime _validTill;

    public PasswordResetRequestCannotBeConfirmedAfterExpirationRule(DateTime validTill)
    {
        _validTill = validTill;
    }

    public bool IsBroken() => _validTill < DateTime.UtcNow;

    public string MessageTemplate => "Password reset request cannot be confirmed because it was already expired";
}
