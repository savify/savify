namespace App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;

public interface IUserFinanceTrackingSettingsRepository
{
    Task AddAsync(CreateUserFinanceTrackingSettings settings);

    Task<CreateUserFinanceTrackingSettings> GetByIdAsync(UserFinanceTrackingSettingsId id);

    Task<CreateUserFinanceTrackingSettings> GetByUserIdAsync(UserId userId);
}
