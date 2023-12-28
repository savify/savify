using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Users.Tags.Events;

public class UserTagsUpdatedDomainEvent(UserId userId, IEnumerable<string> newTags) : DomainEventBase
{
    public UserId UserId { get; } = userId;

    public IEnumerable<string> NewTags { get; } = newTags;
}
