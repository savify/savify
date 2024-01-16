using App.BuildingBlocks.Tests.IntegrationTests.Probing;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.GetUserFinanceTrackingSettings;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using Dapper;
using Npgsql;

namespace App.IntegrationTests.ConfirmUserRegistration;

[TestFixture]
public class CreateUserFinanceTrackingSettingsTests : TestBase
{
    [Test]
    public async Task CreatesUserFinanceTrackingSettings_WhenUserRegistrationWasConfirmed_Test()
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
            new GetCreatedUserFinanceTrackingSettingsFromFinanceTrackingProbe(userRegistrationId, FinanceTrackingModule));
    }

    private class GetCreatedUserFinanceTrackingSettingsFromFinanceTrackingProbe(
        Guid expectedUserId,
        IFinanceTrackingModule financeTrackingModule)
        : IProbe
    {
        private UserFinanceTrackingSettingsDto? _financeTrackingSettings;

        public bool IsSatisfied()
        {
            if (_financeTrackingSettings != null && _financeTrackingSettings.DefaultCurrency == "PLN")
            {
                return true;
            }

            return false;
        }

        public async Task SampleAsync()
        {
            try
            {
                _financeTrackingSettings = await financeTrackingModule.ExecuteQueryAsync(
                    new GetUserFinanceTrackingSettingsQuery(expectedUserId));
            }
            catch
            {
                // ignored
            }
        }

        public string DescribeFailureTo() => $"Finance Tracking settings for user with id '{expectedUserId}' were not created";
    }

    private async Task<string> GetUserRegistrationConfirmationCode(Guid userRegistrationId)
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT confirmation_code FROM {DatabaseConfiguration.Schema.Name}.user_registrations u WHERE u.id = @userRegistrationId";

        return (await sqlConnection.QuerySingleOrDefaultAsync<string>(sql, new { userRegistrationId }))!;
    }
}
