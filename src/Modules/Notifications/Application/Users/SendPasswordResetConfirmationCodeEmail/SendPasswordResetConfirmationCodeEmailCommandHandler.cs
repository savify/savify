using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Domain.UserNotificationSettings;

namespace App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;

public class SendPasswordResetConfirmationCodeEmailCommandHandler : ICommandHandler<SendPasswordResetConfirmationCodeEmailCommand, Result>
{
    private readonly IUserNotificationSettingsRepository _notificationSettingsRepository;
    
    private readonly IEmailMessageFactory _emailMessageFactory;
    
    private readonly IEmailSender _emailSender;

    public SendPasswordResetConfirmationCodeEmailCommandHandler(
        IUserNotificationSettingsRepository notificationSettingsRepository,
        IEmailMessageFactory emailMessageFactory,
        IEmailSender emailSender)
    {
        _notificationSettingsRepository = notificationSettingsRepository;
        _emailMessageFactory = emailMessageFactory;
        _emailSender = emailSender;
    }

    public async Task<Result> Handle(SendPasswordResetConfirmationCodeEmailCommand command, CancellationToken cancellationToken)
    {
        var notificationSettings = await _notificationSettingsRepository.GetByUserEmailAsync(command.Email);
        
        var emailMessage = _emailMessageFactory.CreateLocalizedEmailMessage(
            notificationSettings.Email,
            "Password reset request",
            new PasswordResetEmailTemplateModel(notificationSettings.Name, command.ConfirmationCode),
            notificationSettings.PreferredLanguage.Value);

        await _emailSender.SendEmailAsync(emailMessage);
        
        return Result.Success;
    }
}
