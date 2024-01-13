using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings.Events;

namespace App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;

public class CreateUserFinanceTrackingSettings : Entity
{
    public UserFinanceTrackingSettingsId Id { get; init; }

    public UserId UserId { get; init; }

    public Currency DefaultCurrency { get; private set; }

    public static CreateUserFinanceTrackingSettings Create(UserId userId, Currency defaultCurrency)
    {
        return new CreateUserFinanceTrackingSettings(userId, defaultCurrency);
    }

    public void UpdateDefaultCurrency(Currency newDefaultCurrency)
    {
        DefaultCurrency = newDefaultCurrency;

        AddDomainEvent(new UsersDefaultCurrencyUpdatedDomainEvent(UserId, DefaultCurrency));
    }

    private CreateUserFinanceTrackingSettings(UserId userId, Currency defaultCurrency)
    {
        Id = new UserFinanceTrackingSettingsId(Guid.NewGuid());
        UserId = userId;
        DefaultCurrency = defaultCurrency;
    }

    private CreateUserFinanceTrackingSettings() { }
}
