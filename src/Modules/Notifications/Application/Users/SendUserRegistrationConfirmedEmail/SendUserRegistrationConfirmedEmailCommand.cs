using App.Modules.Notifications.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;

public class SendUserRegistrationConfirmedEmailCommand : CommandBase<Result>
{
    [JsonConstructor]
    public SendUserRegistrationConfirmedEmailCommand(Guid id, string name, string email, string language) : base(id)
    {
        Name = name;
        Email = email;
        Language = language;
    }

    internal string Name { get; }

    internal string Email { get; }
    
    internal string Language { get; }
}
