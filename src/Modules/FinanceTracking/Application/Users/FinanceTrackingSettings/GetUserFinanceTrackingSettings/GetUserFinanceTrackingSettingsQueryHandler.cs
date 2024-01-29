using App.BuildingBlocks.Application.Data;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.GetUserFinanceTrackingSettings;

internal class GetUserFinanceTrackingSettingsQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetUserFinanceTrackingSettingsQuery, UserFinanceTrackingSettingsDto?>
{
    public async Task<UserFinanceTrackingSettingsDto?> Handle(GetUserFinanceTrackingSettingsQuery query, CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT s.currency AS defaultCurrency
                   FROM {DatabaseConfiguration.Schema.Name}.user_finance_tracking_settings s
                   WHERE s.user_id = @UserId
                   """;

        var settings = await connection.QuerySingleOrDefaultAsync<UserFinanceTrackingSettingsDto>(sql, new { query.UserId });

        return settings;
    }
}
