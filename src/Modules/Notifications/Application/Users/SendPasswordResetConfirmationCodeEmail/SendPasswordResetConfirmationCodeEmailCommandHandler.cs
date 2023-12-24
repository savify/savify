using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Domain.UserNotificationSettings;

namespace App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;

internal class SendPasswordResetConfirmationCodeEmailCommandHandler(
    IUserNotificationSettingsRepository notificationSettingsRepository,
    IEmailMessageFactory emailMessageFactory,
    IEmailSender emailSender)
    : ICommandHandler<SendPasswordResetConfirmationCodeEmailCommand>
{
    public async Task Handle(SendPasswordResetConfirmationCodeEmailCommand command, CancellationToken cancellationToken)
    {
        var notificationSettings = await notificationSettingsRepository.GetByUserEmailAsync(command.Email);

        var emailMessage = emailMessageFactory.CreateLocalizedEmailMessage(
            notificationSettings.Email,
            "Password reset request",
            new PasswordResetEmailTemplateModel(notificationSettings.Name, command.ConfirmationCode),
            notificationSettings.PreferredLanguage.Value);

        await emailSender.SendEmailAsync(emailMessage);
    }
}
