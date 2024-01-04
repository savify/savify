namespace App.Modules.FinanceTracking.Application.UserTags.GetUserTags;

public class UserTagsDto
{
    public required IEnumerable<string> Values { get; init; }
}
