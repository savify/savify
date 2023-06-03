using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

public class RenewUserRegistrationCommand : CommandBase<Result>
{
    public Guid UserRegistrationId { get; }

    public RenewUserRegistrationCommand(Guid userRegistrationId)
    {
        UserRegistrationId = userRegistrationId;
    }
}
