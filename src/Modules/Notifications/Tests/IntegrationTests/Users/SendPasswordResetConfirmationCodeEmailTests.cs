using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;
using App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;
using NSubstitute.ReceivedExtensions;
using NSubstitute;

namespace App.Modules.Notifications.IntegrationTests.Users;

[TestFixture]
public class SendPasswordResetConfirmationCodeEmailTests : TestBase
{
    [Test]
    public async Task SendUserRegistrationConfirmationEmailCommand_Test()
    {
        await NotificationsModule.ExecuteCommandAsync(new CreateNotificationSettingsCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Name",
            "test@email.com",
            "en"));
        
        await NotificationsModule.ExecuteCommandAsync(new SendPasswordResetConfirmationCodeEmailCommand(
            Guid.NewGuid(),
            "test@email.com", 
            "ABC123"));
        
        await EmailSender.Received(Quantity.Exactly(1)).SendEmailAsync(Arg.Is<EmailMessage>(e => e.To.Contains("test@email.com")));
    }
    
    // TODO: add test when notification settings does not exist
}
