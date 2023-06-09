using App.BuildingBlocks.Application.Data;
using App.Modules.Notifications.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Notifications.Application.UserNotificationSettings.GetUserNotificationSettings;

public class GetUserNotificationSettingsQueryHandler : IQueryHandler<GetUserNotificationSettingsQuery, UserNotificationSettingsDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUserNotificationSettingsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<UserNotificationSettingsDto> Handle(GetUserNotificationSettingsQuery query, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        string sql = "SELECT user_id AS userId, email, name, preferred_language AS preferredLanguage " +
                     "FROM notifications.user_notification_settings WHERE user_id = @UserId";

        return  await connection.QuerySingleOrDefaultAsync<UserNotificationSettingsDto>(sql, new { query.UserId });
    }
}
