using App.Modules.Notifications.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;

[method: JsonConstructor]
public class SendPasswordResetConfirmationCodeEmailCommand(
    Guid id,
    Guid correlationId,
    string email,
    string confirmationCode)
    : InternalCommandBase(id, correlationId)
{
    internal string Email { get; } = email;

    internal string ConfirmationCode { get; } = confirmationCode;
}
