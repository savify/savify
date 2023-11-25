using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmedEmail;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace App.Modules.Notifications.IntegrationTests.Users;

[TestFixture]
public class SendUserRegistrationConfirmedEmailTests : TestBase
{
    [Test]
    public async Task SendUserRegistrationConfirmedEmailCommand_Test()
    {
        await NotificationsModule.ExecuteCommandAsync(new SendUserRegistrationConfirmedEmailCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Name",
            "test@email.com",
            "en"));

        await EmailSender.Received(Quantity.Exactly(1)).SendEmailAsync(Arg.Is<EmailMessage>(e => e.To.Contains("test@email.com")));
    }
}
