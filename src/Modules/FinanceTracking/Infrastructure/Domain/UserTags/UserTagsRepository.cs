using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Users.Tags;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.UserTags;

internal class UserTagsRepository(FinanceTrackingContext context) : IUserTagsRepository
{
    public async Task AddAsync(FinanceTracking.Domain.Users.Tags.UserTags userTags)
    {
        await context.UserTags.AddAsync(userTags);
    }

    public async Task<FinanceTracking.Domain.Users.Tags.UserTags?> GetByUserIdOrDefaultAsync(UserId id)
    {
        return await context.UserTags.SingleOrDefaultAsync(userTags => userTags.UserId == id);
    }
}
