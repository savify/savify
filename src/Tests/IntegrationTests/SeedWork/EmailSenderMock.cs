using App.Modules.Notifications.Application.Emails;

namespace App.IntegrationTests.SeedWork;

public class EmailSenderMock : IEmailSender
{
    public List<EmailMessage> SentEmails { get; } = new();

    public Task SendEmailAsync(EmailMessage emailMessage)
    {
        SentEmails.Add(emailMessage);
        return Task.CompletedTask;
    }
}
