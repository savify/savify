using App.BuildingBlocks.Tests.IntegrationTests.Probing;
using App.Modules.Notifications.Application.Emails;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

namespace App.IntegrationTests.RenewUserRegistration;

[TestFixture]
public class RenewUserRegistrationTests : TestBase
{
    [Test]
    public async Task SendUserRegistrationRenewalEmail_WhenUserRegistrationWasRenewed_Test()
    {
        var userRegistrationId = await UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            "test@email.com",
            "Test1234!",
            "Name",
            "PL",
            "en"
        ));

        await UserAccessModule.ExecuteCommandAsync(new RenewUserRegistrationCommand(userRegistrationId));

        await AssertEventually(
            new GetUserRegistrationRenewalEmailProbe(
                "test@email.com",
                "Savify - New confirmation code requested",
                EmailSender), 20000);
    }

    private class GetUserRegistrationRenewalEmailProbe(
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

        public string DescribeFailureTo() => $"User registration renewal email was not sent to '{expectedRecipientEmailAddress}'";
    }
}
