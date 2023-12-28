using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
public class GetUserTagsQuery(Guid userId) : QueryBase<UserTagsDto>
{
    public Guid UserId { get; } = userId;
}
