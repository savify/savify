using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Users.FinanceTrackingSettings;

public class UserFinanceTrackingSettingsRepository(FinanceTrackingContext context) : IUserFinanceTrackingSettingsRepository
{
    public async Task AddAsync(UserFinanceTrackingSettings settings)
    {
        await context.AddAsync(settings);
    }

    public async Task<UserFinanceTrackingSettings> GetByIdAsync(UserFinanceTrackingSettingsId id)
    {
        var settings = await context.UserFinanceTrackingSettings.SingleOrDefaultAsync(s => s.Id == id);

        if (settings is null)
        {
            throw new NotFoundRepositoryException<UserFinanceTrackingSettings>(id.Value);
        }

        return settings;
    }

    public async Task<UserFinanceTrackingSettings> GetByUserIdAsync(UserId userId)
    {
        var settings = await context.UserFinanceTrackingSettings.SingleOrDefaultAsync(s => s.UserId == userId);

        if (settings is null)
        {
            throw new NotFoundRepositoryException<UserFinanceTrackingSettings>("Finance Tracking settings for user with ID {0} were not found", [userId.Value]);
        }

        return settings;
    }
}
