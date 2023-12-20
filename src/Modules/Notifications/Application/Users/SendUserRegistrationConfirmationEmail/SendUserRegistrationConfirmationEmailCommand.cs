using App.Modules.Notifications.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;

[method: JsonConstructor]
public class SendUserRegistrationConfirmationEmailCommand(
    Guid id,
    Guid correlationId,
    string name,
    string email,
    string language,
    string confirmationCode)
    : InternalCommandBase(id, correlationId)
{
    internal string Name { get; } = name;

    internal string Email { get; } = email;

    internal string Language { get; } = language;

    internal string ConfirmationCode { get; } = confirmationCode;
}
