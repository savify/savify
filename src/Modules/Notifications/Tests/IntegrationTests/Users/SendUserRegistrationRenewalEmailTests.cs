using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Application.Users.SendUserRegistrationRenewalEmail;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace App.Modules.Notifications.IntegrationTests.Users;

[TestFixture]
public class SendUserRegistrationRenewalEmailTests : TestBase
{
    [Test]
    public async Task SendUserRegistrationRenewalEmailCommand_Test()
    {
        await NotificationsModule.ExecuteCommandAsync(new SendUserRegistrationRenewalEmailCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Name",
            "test@email.com",
            "en",
            "ABC123"));

        await EmailSender.Received(Quantity.Exactly(1)).SendEmailAsync(Arg.Is<EmailMessage>(e => e.To.Contains("test@email.com")));
    }
}
