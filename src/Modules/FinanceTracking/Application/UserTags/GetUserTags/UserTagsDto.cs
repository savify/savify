namespace App.Modules.FinanceTracking.Application.UserTags.GetUserTags;

public class UserTagsDto
{
    public required Guid UserId { get; init; }

    public required IEnumerable<string> Tags { get; init; }
}
