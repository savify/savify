using App.Modules.Banks.Application.Banks.Internal.GetBanks;
using App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;
using App.Modules.Banks.IntegrationTests.SeedWork;

namespace App.Modules.Banks.IntegrationTests.Banks;

[TestFixture]
public class GetBanksInternalQueryTests : TestBase
{
    [Test]
    public async Task GetBanksQuery_ReturnsListOfBanks()
    {
        await CreateBanksAsync();
        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());

        Assert.That(banks.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetBanksQuery_WhenThereAreNoBanks_ReturnsEmptyList()
    {
        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());

        Assert.That(banks, Is.Empty);
    }

    private async Task CreateBanksAsync()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersSuccessfulResponse();
        await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));
    }
}
