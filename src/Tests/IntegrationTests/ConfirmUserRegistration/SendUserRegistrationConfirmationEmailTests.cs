using App.BuildingBlocks.Tests.IntegrationTests.Probing;
using App.Modules.Notifications.Application.Emails;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using Dapper;
using Npgsql;

namespace App.IntegrationTests.ConfirmUserRegistration;

[TestFixture]
public class SendUserRegistrationConfirmationEmailTests : TestBase
{
    [Test]
    public async Task SendUserRegistrationConfirmedEmail_WhenUserRegistrationWasConfirmed_Test()
    {
        var userRegistrationId = await UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
            "test@email.com",
            "Test1234!",
            "Name",
            "PL",
            "en"
        ));
        var confirmationCode = await GetUserRegistrationConfirmationCode(userRegistrationId);

        await UserAccessModule.ExecuteCommandAsync(new ConfirmUserRegistrationCommand(userRegistrationId, confirmationCode));

        await AssertEventually(
            new GetUserRegistrationConfirmedEmailProbe(
                "test@email.com",
                "Savify - You have successfully registered at Savify",
                EmailSender));
    }

    private class GetUserRegistrationConfirmedEmailProbe(
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

        public string DescribeFailureTo() => $"User registration confirmed email was not sent to '{expectedRecipientEmailAddress}'";
    }

    private async Task<string> GetUserRegistrationConfirmationCode(Guid userRegistrationId)
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT confirmation_code FROM {DatabaseConfiguration.Schema.Name}.user_registrations u WHERE u.id = @userRegistrationId";

        return (await sqlConnection.QuerySingleOrDefaultAsync<string>(sql, new { userRegistrationId }))!;
    }
}
