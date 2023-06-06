using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Application.Emails;

namespace App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;

public class SendUserRegistrationConfirmedEmailCommandHandler : ICommandHandler<SendUserRegistrationConfirmedEmailCommand, Result>
{
    private readonly IEmailMessageFactory _emailMessageFactory;
    
    private readonly IEmailSender _emailSender;

    public SendUserRegistrationConfirmedEmailCommandHandler(IEmailMessageFactory emailMessageFactory, IEmailSender emailSender)
    {
        _emailMessageFactory = emailMessageFactory;
        _emailSender = emailSender;
    }

    public async Task<Result> Handle(SendUserRegistrationConfirmedEmailCommand command, CancellationToken cancellationToken)
    {
        var emailMessage = _emailMessageFactory.CreateLocalizedEmailMessage(
            command.Email,
            "You have successfully registered at Savify",
            new RegistrationConfirmedEmailTemplateModel(command.Name),
            command.Language);

        await _emailSender.SendEmailAsync(emailMessage);
        
        return Result.Success;
    }
}
