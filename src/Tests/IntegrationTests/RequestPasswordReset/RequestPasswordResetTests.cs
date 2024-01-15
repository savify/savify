using App.BuildingBlocks.Tests.IntegrationTests.Probing;
using App.Modules.Notifications.Application.Emails;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;
using App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using Dapper;
using Npgsql;

namespace App.IntegrationTests.RequestPasswordReset;

[TestFixture]
public class RequestPasswordResetTests : TestBase
{
    [Test]
    public async Task SendPasswordResetConfirmationCodeEmail_WhenPasswordResetWasRequested_Test()
    {
        await RegisterNewUser();

        await UserAccessModule.ExecuteCommandAsync(new RequestPasswordResetCommand("test@email.com"));

        await AssertEventually(
            new GetPasswordResetConfirmationCodeEmailProbe(
                "test@email.com",
                "Savify - Password reset request",
                EmailSender));
    }

    private class GetPasswordResetConfirmationCodeEmailProbe(
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

        public string DescribeFailureTo() => $"Password reset confirmation email was not sent to '{expectedRecipientEmailAddress}'";
    }

    private async Task RegisterNewUser()
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
    }

    private async Task<string> GetUserRegistrationConfirmationCode(Guid userRegistrationId)
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT confirmation_code FROM {DatabaseConfiguration.Schema.Name}.user_registrations u WHERE u.id = @userRegistrationId";

        return (await sqlConnection.QuerySingleOrDefaultAsync<string>(sql, new { userRegistrationId }))!;
    }
}
