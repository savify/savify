using App.BuildingBlocks.Application.Emails;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace App.BuildingBlocks.Infrastructure.Emails;

public class EmailSender : IEmailSender
{
    private readonly EmailMessageMapper _emailMessageMapper;
    private readonly EmailConfiguration _configuration;
    private readonly ILogger _logger;

    public EmailSender(EmailMessageMapper emailMessageMapper, EmailConfiguration configuration, ILogger logger)
    {
        _emailMessageMapper = emailMessageMapper;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var message = _emailMessageMapper.MapToMimeMessage(emailMessage);

        await SendAsync(message);
    }

    private async Task SendAsync(MimeMessage message)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_configuration.Host, _configuration.Port, _configuration.UseSsl);
                await client.SendAsync(message);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Email sending failed; {Message}", e.Message);
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
