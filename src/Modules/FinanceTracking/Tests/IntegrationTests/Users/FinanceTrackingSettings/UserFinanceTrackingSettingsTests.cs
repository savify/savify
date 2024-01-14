using App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.CreateUserFinanceTrackingSettings;
using App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.GetUserFinanceTrackingSettings;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Users.FinanceTrackingSettings;

[TestFixture]
public class UserFinanceTrackingSettingsTests : TestBase
{
    [Test]
    [TestCase("PL", "PLN")]
    [TestCase("UA", "UAH")]
    [TestCase("US", "USD")]
    [TestCase("GB", "GBP")]
    public async Task CreateUserFinanceTrackingSettingsCommand_CreatesFinanceTrackingSettingsForUser(string countryCode, string currencyCode)
    {
        var userId = Guid.NewGuid();

        await FinanceTrackingModule.ExecuteCommandAsync(new CreateUserFinanceTrackingSettingsCommand(
            id: Guid.NewGuid(),
            correlationId: Guid.NewGuid(),
            userId: userId,
            countryCode: countryCode));

        var settings = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserFinanceTrackingSettingsQuery(userId));

        Assert.That(settings, Is.Not.Null);
        Assert.That(settings!.DefaultCurrency, Is.EqualTo(currencyCode));
    }
}
