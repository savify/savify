using App.Modules.Banks.Application.Banks.Internal.GetBanks;
using App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.IntegrationTests.SeedWork;

namespace App.Modules.Banks.IntegrationTests.BanksSynchronisationProcessing;

[TestFixture]
public class SynchroniseBanksCommandTests : TestBase
{
    [Test]
    public async Task SynchroniseBanksCommand_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersSuccessfulResponse();

        var result = await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));

        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());

        Assert.That(result.Status, Is.EqualTo(BanksSynchronisationProcessStatus.Finished.Value));
        Assert.That(banks.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task SynchroniseBanksCommand_WhenErrorAtProviderOccurred_WillReturnFailedStatus_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersErrorResponse();

        var result = await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));

        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());

        Assert.That(result.Status, Is.EqualTo(BanksSynchronisationProcessStatus.Failed.Value));
        Assert.That(banks.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task SynchroniseBanksCommand_WhenSomeBankWasUpdated_WillUpdateThisBank_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersSuccessfulResponse();
        await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));

        SaltEdgeHttpClientMocker.MockFetchUpdatedProvidersSuccessfulResponse();

        var result = await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));
        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());
        var updatedBank = banks.Single(b => b.Name == "Bank name 1");

        Assert.That(result.Status, Is.EqualTo(BanksSynchronisationProcessStatus.Finished.Value));
        Assert.That(banks.Count, Is.EqualTo(2));
        Assert.That(updatedBank.Status, Is.EqualTo(BankStatus.Disabled.Value));
        Assert.That(updatedBank.IsRegulated, Is.False);
        Assert.That(updatedBank.MaxConsentDays, Is.EqualTo(30));
        Assert.That(updatedBank.DefaultLogoUrl, Is.EqualTo("https://external/banks/bank-new.svg"));
    }

    [Test]
    public async Task SynchroniseBanksCommand_WhenSomeBankWasDisabled_WillDisableThisBank_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersSuccessfulResponse();
        await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));

        SaltEdgeHttpClientMocker.MockFetchUpdatedProvidersSuccessfulResponse();

        var result = await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));
        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());
        var disabledBank = banks.Single(b => b.Name == "Bank name 2");

        Assert.That(result.Status, Is.EqualTo(BanksSynchronisationProcessStatus.Finished.Value));
        Assert.That(banks.Count, Is.EqualTo(2));
        Assert.That(disabledBank.Status, Is.EqualTo(BankStatus.Disabled.Value));
    }
}
