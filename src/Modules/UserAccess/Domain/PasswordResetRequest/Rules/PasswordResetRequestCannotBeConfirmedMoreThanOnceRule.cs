using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Rules;

public class PasswordResetRequestCannotBeConfirmedMoreThanOnceRule : IBusinessRule
{
    private readonly PasswordResetRequestStatus _actualStatus;

    public PasswordResetRequestCannotBeConfirmedMoreThanOnceRule(PasswordResetRequestStatus actualStatus)
    {
        _actualStatus = actualStatus;
    }

    public bool IsBroken() => _actualStatus == PasswordResetRequestStatus.Confirmed;

    public string MessageTemplate => "Password reset request was already confirmed";
}
