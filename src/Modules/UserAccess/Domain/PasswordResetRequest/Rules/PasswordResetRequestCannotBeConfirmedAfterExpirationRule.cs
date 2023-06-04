using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Rules;

public class PasswordResetRequestCannotBeConfirmedAfterExpirationRule : IBusinessRule
{
    private readonly DateTime _expiresAt;

    public PasswordResetRequestCannotBeConfirmedAfterExpirationRule(DateTime expiresAt)
    {
        _expiresAt = expiresAt;
    }

    public bool IsBroken() => _expiresAt < DateTime.Now;

    public string MessageTemplate => "Password reset request cannot be confirmed because it was already expired";
}
