using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Contracts;
using Newtonsoft.Json;

namespace App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;

public class SendPasswordResetConfirmationCodeEmailCommand : InternalCommandBase<Result>
{
    [JsonConstructor]
    public SendPasswordResetConfirmationCodeEmailCommand(Guid id, string email, string confirmationCode) : base(id)
    {
        Email = email;
        ConfirmationCode = confirmationCode;
    }

    internal string Email { get; }

    internal string ConfirmationCode { get; }
}
