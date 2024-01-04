using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Users.Tags;

public class UserTags : Entity
{
    public UserId UserId { get; private init; }

    private List<string> _tags;

    public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

    public static UserTags Create(UserId userId) => new UserTags(userId);

    public void Update(IEnumerable<string> tags)
    {
        var newTags = tags.Except(_tags).ToArray();
        _tags.AddRange(newTags);
    }

    private UserTags(UserId userId)
    {
        UserId = userId;
        _tags = new List<string>();
    }

    private UserTags() { }
}
