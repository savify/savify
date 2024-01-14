namespace App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;

public interface IUserFinanceTrackingSettingsRepository
{
    Task AddAsync(UserFinanceTrackingSettings settings);

    Task<UserFinanceTrackingSettings> GetByIdAsync(UserFinanceTrackingSettingsId id);

    Task<UserFinanceTrackingSettings> GetByUserIdAsync(UserId userId);
}
