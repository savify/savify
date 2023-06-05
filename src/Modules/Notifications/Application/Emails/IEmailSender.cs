namespace App.Modules.Notifications.Application.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
