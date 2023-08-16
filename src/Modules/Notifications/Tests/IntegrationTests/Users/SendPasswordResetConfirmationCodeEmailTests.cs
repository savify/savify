using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Application.UserNotificationSettings.CreateUserNotificationSettings;
using App.Modules.Notifications.Application.Users.SendPasswordResetConfirmationCodeEmail;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

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

    [Test]
    public async Task SendUserRegistrationConfirmationEmailCommand_WhenNotificationSettingsNotExist_Fails()
    {
        var exception = Assert.ThrowsAsync<NotFoundRepositoryException<Domain.UserNotificationSettings.UserNotificationSettings>>(
            async Task () =>
            {
                await NotificationsModule.ExecuteCommandAsync(new SendPasswordResetConfirmationCodeEmailCommand(
                    Guid.NewGuid(),
                    "test@email.com",
                    "ABC123"));
            });

        Assert.That(exception.Message, Is.EqualTo(string.Format(
            "UserNotificationSettings for user with email '{0}' was not found",
            new object[] { "test@email.com" })));
    }
}
