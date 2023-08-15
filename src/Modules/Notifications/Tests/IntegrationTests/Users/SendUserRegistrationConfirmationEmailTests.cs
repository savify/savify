using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;
using NSubstitute.ReceivedExtensions;
using NSubstitute;

namespace App.Modules.Notifications.IntegrationTests.Users;

[TestFixture]
public class SendUserRegistrationConfirmationEmailTests : TestBase
{
    [Test]
    public async Task SendUserRegistrationConfirmationEmailCommand_Test()
    {
        await NotificationsModule.ExecuteCommandAsync(new SendUserRegistrationConfirmationEmailCommand(
            Guid.NewGuid(),
            "Name",
            "test@email.com",
            "en",
            "ABC123"));
            
        await EmailSender.Received(Quantity.Exactly(1)).SendEmailAsync(Arg.Is<EmailMessage>(e => e.To.Contains("test@email.com")));
    }
}