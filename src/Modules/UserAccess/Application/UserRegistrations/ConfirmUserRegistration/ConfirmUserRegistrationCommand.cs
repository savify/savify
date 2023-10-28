using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

public class ConfirmUserRegistrationCommand : CommandBase
{
    public Guid UserRegistrationId { get; }

    [LogMasked]
    public string ConfirmationCode { get; }

    public ConfirmUserRegistrationCommand(Guid userRegistrationId, string confirmationCode)
    {
        UserRegistrationId = userRegistrationId;
        ConfirmationCode = confirmationCode;
    }
}
