using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Emails;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationRenewalEmail;

internal class SendUserRegistrationRenewalEmailCommandHandler(
    IEmailMessageFactory emailMessageFactory,
    IEmailSender emailSender)
    : ICommandHandler<SendUserRegistrationRenewalEmailCommand>
{
    public async Task Handle(SendUserRegistrationRenewalEmailCommand command, CancellationToken cancellationToken)
    {
        var emailMessage = emailMessageFactory.CreateLocalizedEmailMessage(
            command.Email,
            "New confirmation code requested",
            new RegistrationRenewalEmailTemplateModel(command.Name, command.ConfirmationCode),
            command.Language);

        await emailSender.SendEmailAsync(emailMessage);
    }
}
