using App.Modules.UserAccess.Application.Contracts;
using Destructurama.Attributed;

namespace App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;

public class ConfirmUserRegistrationCommand(Guid userRegistrationId, string confirmationCode) : CommandBase
{
    public Guid UserRegistrationId { get; } = userRegistrationId;

    [LogMasked]
    public string ConfirmationCode { get; } = confirmationCode;
}
