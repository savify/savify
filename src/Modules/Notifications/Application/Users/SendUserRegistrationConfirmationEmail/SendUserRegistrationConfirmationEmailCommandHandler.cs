using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Emails;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;

internal class SendUserRegistrationConfirmationEmailCommandHandler(
    IEmailMessageFactory emailMessageFactory,
    IEmailSender emailSender)
    : ICommandHandler<SendUserRegistrationConfirmationEmailCommand>
{
    public async Task Handle(SendUserRegistrationConfirmationEmailCommand command, CancellationToken cancellationToken)
    {
        var emailMessage = emailMessageFactory.CreateLocalizedEmailMessage(
            command.Email,
            "Welcome - Please confirm your registration",
            new RegistrationConfirmationEmailTemplateModel(command.Name, command.ConfirmationCode),
            command.Language);

        await emailSender.SendEmailAsync(emailMessage);
    }
}
