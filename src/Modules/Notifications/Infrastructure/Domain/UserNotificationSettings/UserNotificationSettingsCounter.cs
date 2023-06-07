using App.BuildingBlocks.Application.Data;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using Dapper;

namespace App.Modules.Notifications.Infrastructure.Domain.UserNotificationSettings;

public class UserNotificationSettingsCounter : IUserNotificationSettingsCounter
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public UserNotificationSettingsCounter(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public int CountNotificationSettingsWithUserId(UserId userId)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = "SELECT COUNT(*) FROM notifications.user_notification_settings WHERE user_id = @userId";

        return connection.QuerySingle<int>(sql, new {userId = userId.Value});
    }
}
