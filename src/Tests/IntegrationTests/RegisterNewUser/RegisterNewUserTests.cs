using App.BuildingBlocks.Tests.IntegrationTests.Probing;
using App.Modules.Notifications.Application.Emails;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;

namespace App.IntegrationTests.RegisterNewUser;

[TestFixture]
public class RegisterNewUserTests : TestBase
{
    [Test]
    public async Task SendUserRegistrationConfirmationEmail_WhenUserRegistrationWasCreated_Test()
    {
        await UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            "test@email.com",
            "Test1234!",
            "Name",
            "PL",
            "en"
        ));

        await AssertEventually(
            new GetUserRegistrationConfirmationEmailProbe(
                "test@email.com",
                "Savify - Welcome - Please confirm your registration",
                EmailSender));
    }

    private class GetUserRegistrationConfirmationEmailProbe(
        string expectedRecipientEmailAddress,
        string expectedEmailSubject,
        IEmailSender emailSender)
        : IProbe
    {
        private EmailMessage? _emailMessage;

        public bool IsSatisfied()
        {
            if (_emailMessage != null)
            {
                return true;
            }

            return false;
        }

        public Task SampleAsync()
        {
            try
            {
                _emailMessage = ((EmailSenderMock)emailSender).SentEmails.SingleOrDefault(e =>
                    e.To.Any(address => address == expectedRecipientEmailAddress) &&
                    e.Subject == expectedEmailSubject);
            }
            catch
            {
                // ignored
            }

            return Task.CompletedTask;
        }

        public string DescribeFailureTo() => $"User registration confirmation email was not sent to '{expectedRecipientEmailAddress}'";
    }
}
