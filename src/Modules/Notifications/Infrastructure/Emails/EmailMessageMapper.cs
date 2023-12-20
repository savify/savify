using App.Modules.Notifications.Application.Emails;
using MimeKit;

namespace App.Modules.Notifications.Infrastructure.Emails;

public class EmailMessageMapper(EmailConfiguration configuration)
{
    public MimeMessage MapToMimeMessage(EmailMessage emailMessage)
    {
        var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(new MailboxAddress(configuration.FromName, configuration.FromEmail));
        mimeMessage.To.AddRange(emailMessage.To.Select(x => new MailboxAddress(null, x)));
        mimeMessage.Subject = emailMessage.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = emailMessage.Content
        };
        mimeMessage.Body = bodyBuilder.ToMessageBody();

        return mimeMessage;
    }
}
