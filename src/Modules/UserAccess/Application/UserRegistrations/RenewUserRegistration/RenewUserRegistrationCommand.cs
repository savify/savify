using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

public class RenewUserRegistrationCommand : CommandBase
{
    public Guid UserRegistrationId { get; }

    public RenewUserRegistrationCommand(Guid userRegistrationId)
    {
        UserRegistrationId = userRegistrationId;
    }
}
