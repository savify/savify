using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;

namespace App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings.Events;

public class UsersDefaultCurrencyUpdatedDomainEvent(UserId userId, Currency newDefaultCurrency) : DomainEventBase
{
    public UserId UserId { get; } = userId;

    public Currency NewDefaultCurrency { get; } = newDefaultCurrency;
}
