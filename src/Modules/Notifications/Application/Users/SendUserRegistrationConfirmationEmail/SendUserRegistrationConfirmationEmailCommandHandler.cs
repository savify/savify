using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Emails;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;

internal class SendUserRegistrationConfirmationEmailCommandHandler : ICommandHandler<SendUserRegistrationConfirmationEmailCommand>
{
    private readonly IEmailMessageFactory _emailMessageFactory;

    private readonly IEmailSender _emailSender;

    public SendUserRegistrationConfirmationEmailCommandHandler(IEmailMessageFactory emailMessageFactory, IEmailSender emailSender)
    {
        _emailMessageFactory = emailMessageFactory;
        _emailSender = emailSender;
    }

    public async Task Handle(SendUserRegistrationConfirmationEmailCommand command, CancellationToken cancellationToken)
    {
        var emailMessage = _emailMessageFactory.CreateLocalizedEmailMessage(
            command.Email,
            "Welcome - Please confirm your registration",
            new RegistrationConfirmationEmailTemplateModel(command.Name, command.ConfirmationCode),
            command.Language);

        await _emailSender.SendEmailAsync(emailMessage);
    }
}
