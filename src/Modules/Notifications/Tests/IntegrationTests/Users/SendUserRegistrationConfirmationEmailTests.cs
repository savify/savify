using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Application.Users.SendUserRegistrationConfirmationEmail;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.ReceivedExtensions;
using NSubstitute;

namespace App.Modules.Notifications.IntegrationTests.Users
{
    [TestFixture]
    public class SendUserRegistrationConfirmationEmailTests : TestBase
    {
        [Test]
        public async Task SendUserRegistrationConfirmationEmailCommand_Test()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            await NotificationsModule.ExecuteCommandAsync(new SendUserRegistrationConfirmationEmailCommand(
                Guid.NewGuid(),
                "Name",
                "test@email.com",
                "en",
                "ABC123"));

            //Other way to check the emails sending should be found here. IEmailSender is not mocked, but injected directly from DI, so the following line does not work as expected.
            await emailSender.Received(Quantity.Exactly(1)).SendEmailAsync(Arg.Is<EmailMessage>(e => e.To.Contains("test@email.com")));
        }
    }
}
