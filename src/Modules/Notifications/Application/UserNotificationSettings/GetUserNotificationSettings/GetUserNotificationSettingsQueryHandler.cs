using App.BuildingBlocks.Application.Data;
using App.Modules.Notifications.Application.Configuration.Data;
using App.Modules.Notifications.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.Notifications.Application.UserNotificationSettings.GetUserNotificationSettings;

internal class GetUserNotificationSettingsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<GetUserNotificationSettingsQuery, UserNotificationSettingsDto?>
{
    public async Task<UserNotificationSettingsDto?> Handle(GetUserNotificationSettingsQuery query, CancellationToken cancellationToken)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();

        string sql = $"""
                      SELECT user_id AS userId, email, name, preferred_language AS preferredLanguage
                      FROM {DatabaseConfiguration.Schema.Name}.user_notification_settings
                      WHERE user_id = @UserId
                      """;

        return await connection.QuerySingleOrDefaultAsync<UserNotificationSettingsDto>(sql, new { query.UserId });
    }
}
