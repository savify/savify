using App.BuildingBlocks.Tests.IntegrationTests.Probing;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Application.UserNotificationSettings.GetUserNotificationSettings;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;
using App.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using Dapper;
using Npgsql;

namespace App.IntegrationTests.ConfirmUserRegistration;

[TestFixture]
public class CreateUserNotificationSettingsTests : TestBase
{
    [Test]
    public async Task CreateUserNotificationSettings_WhenUserRegistrationWasConfirmed_Test()
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
            new GetCreatedUserNotificationSettingsFromNotificationsProbe(userRegistrationId, NotificationsModule));
    }

    private class GetCreatedUserNotificationSettingsFromNotificationsProbe(
        Guid expectedUserId,
        INotificationsModule notificationsModule)
        : IProbe
    {
        private UserNotificationSettingsDto? _notificationSettings;

        public bool IsSatisfied()
        {
            if (_notificationSettings != null && _notificationSettings.UserId == expectedUserId)
            {
                return true;
            }

            return false;
        }

        public async Task SampleAsync()
        {
            try
            {
                _notificationSettings = await notificationsModule.ExecuteQueryAsync(
                    new GetUserNotificationSettingsQuery(expectedUserId));
            }
            catch
            {
                // ignored
            }
        }

        public string DescribeFailureTo() => $"Notification settings for user with id '{expectedUserId}' were not created";
    }

    private async Task<string> GetUserRegistrationConfirmationCode(Guid userRegistrationId)
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT confirmation_code FROM {DatabaseConfiguration.Schema.Name}.user_registrations u WHERE u.id = @userRegistrationId";

        return (await sqlConnection.QuerySingleOrDefaultAsync<string>(sql, new { userRegistrationId }))!;
    }
}
