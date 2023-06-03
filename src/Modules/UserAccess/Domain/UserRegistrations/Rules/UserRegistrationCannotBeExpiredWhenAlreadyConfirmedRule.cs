using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Rules;

public class UserRegistrationCannotBeExpiredWhenAlreadyConfirmedRule : IBusinessRule
{
    private readonly UserRegistrationStatus _actualRegistrationStatus;

    public UserRegistrationCannotBeExpiredWhenAlreadyConfirmedRule(UserRegistrationStatus actualRegistrationStatus)
    {
        _actualRegistrationStatus = actualRegistrationStatus;
    }

    public bool IsBroken() => _actualRegistrationStatus == UserRegistrationStatus.Confirmed;

    public string MessageTemplate => "User Registration cannot be expired because it was already confirmed";
}
