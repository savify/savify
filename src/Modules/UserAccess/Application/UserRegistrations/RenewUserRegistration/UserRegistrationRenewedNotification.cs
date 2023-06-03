using App.BuildingBlocks.Application.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using Newtonsoft.Json;

namespace App.Modules.UserAccess.Application.UserRegistrations.RenewUserRegistration;

public class UserRegistrationRenewedNotification : DomainEventNotificationBase<UserRegistrationRenewedDomainEvent>
{
    [JsonConstructor]
    public UserRegistrationRenewedNotification(Guid id, UserRegistrationRenewedDomainEvent domainEvent) : base(id, domainEvent)
    {
    }
}
