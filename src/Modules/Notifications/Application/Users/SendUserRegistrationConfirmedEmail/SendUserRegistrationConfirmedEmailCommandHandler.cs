using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Emails;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;

internal class SendUserRegistrationConfirmedEmailCommandHandler(
    IEmailMessageFactory emailMessageFactory,
    IEmailSender emailSender)
    : ICommandHandler<SendUserRegistrationConfirmedEmailCommand>
{
    public async Task Handle(SendUserRegistrationConfirmedEmailCommand command, CancellationToken cancellationToken)
    {
        var emailMessage = emailMessageFactory.CreateLocalizedEmailMessage(
            command.Email,
            "You have successfully registered at Savify",
            new RegistrationConfirmedEmailTemplateModel(command.Name),
            command.Language);

        await emailSender.SendEmailAsync(emailMessage);
    }
}
