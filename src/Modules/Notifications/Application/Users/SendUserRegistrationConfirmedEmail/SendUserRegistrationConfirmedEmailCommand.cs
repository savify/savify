using App.Modules.Notifications.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;

public class SendUserRegistrationConfirmedEmailCommand : InternalCommandBase
{
    [JsonConstructor]
    public SendUserRegistrationConfirmedEmailCommand(
        Guid id,
        Guid correlationId,
        string name,
        string email,
        string language) : base(id, correlationId)
    {
        Name = name;
        Email = email;
        Language = language;
    }

    internal string Name { get; }

    internal string Email { get; }

    internal string Language { get; }
}
