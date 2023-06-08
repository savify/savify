using App.Modules.Notifications.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;

public class SendUserRegistrationConfirmationEmailCommand : CommandBase<Result>
{
    [JsonConstructor]
    public SendUserRegistrationConfirmationEmailCommand(Guid id, string name, string email, string language, string confirmationCode) : base(id)
    {
        Name = name;
        Email = email;
        Language = language;
        ConfirmationCode = confirmationCode;
    }

    internal string Name { get; }

    internal string Email { get; }
    
    internal string Language { get; }

    internal string ConfirmationCode { get; }
}
