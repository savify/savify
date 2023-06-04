using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Rules;

public class UserRegistrationCannotBeConfirmedAfterExpirationRule : IBusinessRule
{
    private readonly DateTime _validTill;

    public UserRegistrationCannotBeConfirmedAfterExpirationRule(DateTime validTill)
    {
        _validTill = validTill;
    }

    public bool IsBroken() => _validTill < DateTime.UtcNow;

    public string MessageTemplate => "User Registration cannot be confirmed because it was already expired";
}
