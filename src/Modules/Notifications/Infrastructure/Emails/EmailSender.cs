using App.Modules.Notifications.Application.Emails;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace App.Modules.Notifications.Infrastructure.Emails;

public class EmailSender(EmailMessageMapper emailMessageMapper, EmailConfiguration configuration, ILogger logger) : IEmailSender
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var message = emailMessageMapper.MapToMimeMessage(emailMessage);

        await SendAsync(message);
    }

    private async Task SendAsync(MimeMessage message)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(configuration.Host, configuration.Port, configuration.UseSsl);
                await client.SendAsync(message);
            }
            catch (Exception e)
            {
                logger.Error(e, "Email sending failed; {Message}", e.Message);
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
