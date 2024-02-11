using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings.Events;

namespace App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;

public class UserFinanceTrackingSettings : Entity
{
    public UserFinanceTrackingSettingsId Id { get; init; }

    public UserId UserId { get; init; }

    public Currency DefaultCurrency { get; private set; }

    public Language PreferredLanguage { get; private set; }

    public static UserFinanceTrackingSettings Create(UserId userId, Currency defaultCurrency, Language preferredLanguage)
    {
        return new UserFinanceTrackingSettings(userId, defaultCurrency, preferredLanguage);
    }

    public void UpdateDefaultCurrency(Currency newDefaultCurrency)
    {
        DefaultCurrency = newDefaultCurrency;

        AddDomainEvent(new UsersDefaultCurrencyUpdatedDomainEvent(UserId, DefaultCurrency));
    }

    private UserFinanceTrackingSettings(UserId userId, Currency defaultCurrency, Language preferredLanguage)
    {
        Id = new UserFinanceTrackingSettingsId(Guid.NewGuid());
        UserId = userId;
        DefaultCurrency = defaultCurrency;
        PreferredLanguage = preferredLanguage;
    }

    private UserFinanceTrackingSettings() { }
}
