using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationRenewalEmail;

public class SendUserRegistrationRenewalEmailCommand : InternalCommandBase<Result>
{
    [JsonConstructor]
    public SendUserRegistrationRenewalEmailCommand(Guid id, string name, string email, string language, string confirmationCode) : base(id)
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
