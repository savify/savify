using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Infrastructure.Emails;

namespace App.Modules.Notifications.UnitTests.Infrastructure;

[TestFixture]
public class EmailMessageMapperTests : UnitTestBase
{
    [Test]
    public void MapToMimeMessage_ShouldMapToMimeMessage()
    {
        var emailMessage = new EmailMessage("to@mail.com", "email subject", "email content");

        var emailMessageMapper = new EmailMessageMapper(new EmailConfiguration
        {
            AppUrl = "http://localhost:8080",
            FromName = "Savify",
            FromEmail = "no-reply@savify.localhost",
            Host = "localhost"
        });

        var mimeMessage = emailMessageMapper.MapToMimeMessage(emailMessage);

        Assert.That(mimeMessage.From.Mailboxes.Any(m => m.Name == "Savify" && m.Address == "no-reply@savify.localhost"));
        Assert.That(mimeMessage.To.Mailboxes.Any(m => m.Address == "to@mail.com"));
        Assert.That(mimeMessage.Subject, Is.EqualTo(emailMessage.Subject));
        Assert.That(mimeMessage.HtmlBody, Is.EqualTo(emailMessage.Content));
    }

    [Test]
    public void MapToMimeMessage_WithMultipleRecipients_ShouldMapToMimeMessage()
    {
        var emailMessage = new EmailMessage(new[] { "to1@mail.com", "to2@mail.com" }, "email subject", "email content");

        var emailMessageMapper = new EmailMessageMapper(new EmailConfiguration
        {
            AppUrl = "http://localhost:8080",
            FromName = "Savify",
            FromEmail = "no-reply@savify.localhost",
            Host = "localhost"
        });

        var mimeMessage = emailMessageMapper.MapToMimeMessage(emailMessage);

        Assert.That(mimeMessage.To.Mailboxes.Any(m => m.Address == "to1@mail.com"));
        Assert.That(mimeMessage.To.Mailboxes.Any(m => m.Address == "to2@mail.com"));
    }
}
