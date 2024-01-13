using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.GetUserFinanceTrackingSettings;

public class GetUserFinanceTrackingSettingsQuery(Guid userId) : QueryBase<UserFinanceTrackingSettingsDto?>
{
    public Guid UserId { get; } = userId;
}
