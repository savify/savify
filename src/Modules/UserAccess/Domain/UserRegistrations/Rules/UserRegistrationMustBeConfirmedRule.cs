using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Rules;

public class UserRegistrationMustBeConfirmedRule : IBusinessRule
{
    private readonly UserRegistrationStatus _actualRegistrationStatus;
    
    internal UserRegistrationMustBeConfirmedRule(UserRegistrationStatus actualRegistrationStatus)
    {
        _actualRegistrationStatus = actualRegistrationStatus;
    }
    
    public bool IsBroken() => _actualRegistrationStatus != UserRegistrationStatus.Confirmed;

    public string MessageTemplate => "User registration was not confirmed yet";
}
