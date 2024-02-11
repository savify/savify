using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings.Events;

namespace App.Modules.FinanceTracking.UnitTests.Users.FinanceTrackingSettings;

[TestFixture]
public class UserFinanceTrackingSettingsTests : UnitTestBase
{
    [Test]
    public void CreatingUserFinanceTrackingSetting_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var defaultCurrency = Currency.From("USD");
        var preferredLaguage = Language.From("en");

        var settings = UserFinanceTrackingSettings.Create(userId, defaultCurrency, preferredLaguage);

        Assert.That(settings.UserId, Is.EqualTo(userId));
        Assert.That(settings.DefaultCurrency, Is.EqualTo(defaultCurrency));
    }

    [Test]
    public void UpdatingDefaultCurrency_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var defaultCurrency = Currency.From("USD");
        var preferredLaguage = Language.From("en");
        var settings = UserFinanceTrackingSettings.Create(userId, defaultCurrency, preferredLaguage);

        var newDefaultCurrency = Currency.From("PLN");
        settings.UpdateDefaultCurrency(newDefaultCurrency);

        var domainEvent = AssertPublishedDomainEvent<UsersDefaultCurrencyUpdatedDomainEvent>(settings);
        Assert.That(domainEvent.UserId, Is.EqualTo(userId));
        Assert.That(domainEvent.NewDefaultCurrency, Is.EqualTo(newDefaultCurrency));
    }
}
