using App.Modules.Notifications.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;

[method: JsonConstructor]
public class SendUserRegistrationConfirmedEmailCommand(
    Guid id,
    Guid correlationId,
    string name,
    string email,
    string language)
    : InternalCommandBase(id, correlationId)
{
    internal string Name { get; } = name;

    internal string Email { get; } = email;

    internal string Language { get; } = language;
}
