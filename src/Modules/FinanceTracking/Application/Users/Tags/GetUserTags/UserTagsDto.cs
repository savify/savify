namespace App.Modules.FinanceTracking.Application.Users.Tags.GetUserTags;

public class UserTagsDto
{
    public required IEnumerable<string> Values { get; init; }
}
