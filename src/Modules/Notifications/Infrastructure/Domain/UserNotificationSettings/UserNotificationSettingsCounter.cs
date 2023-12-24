using App.BuildingBlocks.Application.Data;
using App.Modules.Notifications.Application.Configuration.Data;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using Dapper;

namespace App.Modules.Notifications.Infrastructure.Domain.UserNotificationSettings;

public class UserNotificationSettingsCounter(ISqlConnectionFactory sqlConnectionFactory) : IUserNotificationSettingsCounter
{
    public int CountNotificationSettingsWithUserId(UserId userId)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"SELECT COUNT(*) FROM {DatabaseConfiguration.Schema.Name}.user_notification_settings WHERE user_id = @userId";

        return connection.QuerySingle<int>(sql, new { userId = userId.Value });
    }
}
