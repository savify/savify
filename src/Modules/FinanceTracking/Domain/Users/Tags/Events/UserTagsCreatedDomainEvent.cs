using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Users.Tags.Events;

public class UserTagsCreatedDomainEvent(UserId userId) : DomainEventBase
{
    public UserId UserId { get; } = userId;
}
