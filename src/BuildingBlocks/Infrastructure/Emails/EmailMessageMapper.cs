using App.BuildingBlocks.Application.Emails;
using MimeKit;

namespace App.BuildingBlocks.Infrastructure.Emails;

public class EmailMessageMapper
{
    private readonly EmailConfiguration _configuration;

    public EmailMessageMapper(EmailConfiguration configuration)
    {
        _configuration = configuration;
    }

    public MimeMessage MapToMimeMessage(EmailMessage emailMessage)
    {
        var mimeMessage = new MimeMessage();
        
        mimeMessage.From.Add(new MailboxAddress(_configuration.FromName, _configuration.FromEmail));
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
