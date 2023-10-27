using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Emails;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationRenewalEmail;

internal class SendUserRegistrationRenewalEmailCommandHandler : ICommandHandler<SendUserRegistrationRenewalEmailCommand>
{
    private readonly IEmailMessageFactory _emailMessageFactory;

    private readonly IEmailSender _emailSender;

    public SendUserRegistrationRenewalEmailCommandHandler(IEmailMessageFactory emailMessageFactory, IEmailSender emailSender)
    {
        _emailMessageFactory = emailMessageFactory;
        _emailSender = emailSender;
    }

    public async Task Handle(SendUserRegistrationRenewalEmailCommand command, CancellationToken cancellationToken)
    {
        var emailMessage = _emailMessageFactory.CreateLocalizedEmailMessage(
            command.Email,
            "New confirmation code requested",
            new RegistrationRenewalEmailTemplateModel(command.Name, command.ConfirmationCode),
            command.Language);

        await _emailSender.SendEmailAsync(emailMessage);
    }
}
