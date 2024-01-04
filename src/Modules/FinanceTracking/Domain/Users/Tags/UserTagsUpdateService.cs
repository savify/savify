namespace App.Modules.FinanceTracking.Domain.Users.Tags;
public class UserTagsUpdateService(IUserTagsRepository repository)
{
    public async Task UpdateAsync(UserId userId, IEnumerable<string>? tags)
    {
        var userTags = await repository.GetByUserIdOrDefaultAsync(userId);
        if (userTags is null)
        {
            userTags = UserTags.Create(userId);
            await repository.AddAsync(userTags);
        }

        if (tags is not null)
        {
            userTags.Update(tags);
        }
    }
}
