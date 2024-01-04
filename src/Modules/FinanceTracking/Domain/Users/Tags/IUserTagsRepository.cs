namespace App.Modules.FinanceTracking.Domain.Users.Tags;

public interface IUserTagsRepository
{
    Task<UserTags?> GetByUserIdOrDefaultAsync(UserId id);

    Task AddAsync(UserTags userTags);
}
