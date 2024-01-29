using App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.GetUserFinanceTrackingSettings;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Users.FinanceTrackingSettings;

[TestFixture]
public class GetUserFinanceTrackingSettingsTests : TestBase
{
    [Test]
    public async Task GetUserFinanceTrackingSettings_WhenDoNotExist_ReturnsNull()
    {
        var settings = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserFinanceTrackingSettingsQuery(Guid.NewGuid()));

        Assert.That(settings, Is.Null);
    }
}
