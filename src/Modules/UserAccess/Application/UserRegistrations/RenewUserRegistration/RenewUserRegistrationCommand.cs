using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

public class RenewUserRegistrationCommand(Guid userRegistrationId) : CommandBase
{
    public Guid UserRegistrationId { get; } = userRegistrationId;
}
