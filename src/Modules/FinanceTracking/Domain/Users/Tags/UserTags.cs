using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Users.Tags.Events;

namespace App.Modules.FinanceTracking.Domain.Users.Tags;

public class UserTags : Entity, IAggregateRoot
{
    public UserId UserId { get; private init; }

    private List<string> _tags;

    public static UserTags Create(UserId userId) => new UserTags(userId);

    public void Update(IEnumerable<string> tags)
    {
        var newTags = tags.Except(_tags).ToArray();
        _tags.AddRange(newTags);

        AddDomainEvent(new UserTagsUpdatedDomainEvent(UserId, newTags));
    }

    private UserTags(UserId userId)
    {
        UserId = userId;
        _tags = new List<string>();
    }

    private UserTags()
    { }
}
