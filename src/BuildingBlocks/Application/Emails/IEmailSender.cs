namespace App.BuildingBlocks.Application.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
