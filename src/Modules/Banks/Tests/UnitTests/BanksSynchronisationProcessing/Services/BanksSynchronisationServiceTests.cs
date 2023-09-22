using App.Modules.Banks.Domain;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.ExternalProviders;
using App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge.Providers;

namespace App.Modules.Banks.UnitTests.BanksSynchronisationProcessing.Services;

[TestFixture]
public class BanksSynchronisationServiceTests : UnitTestBase
{
    [Test]
    public async Task SynchroniseAsync_ForEachExternalProvider_WillCreateANewBank()
    {
        var bankSynchronisationProcessId = new BanksSynchronisationProcessId(Guid.NewGuid());
        var externalProviders = GetExternalProviders();

        var integrationService = Substitute.For<ISaltEdgeIntegrationService>();
        integrationService.FetchProvidersAsync().Returns(externalProviders);

        var bankRepository = Substitute.For<IBankRepository>();

        var service = new BanksSynchronisationService(integrationService, bankRepository);

        await service.SynchroniseAsync(bankSynchronisationProcessId);

        await integrationService.Received(1).FetchProvidersAsync();
        await bankRepository.Received(1).AddAsync(Arg.Is<Bank>(b =>
            b.LastBanksSynchronisationProcessId == bankSynchronisationProcessId &&
            b.ExternalId == "external-id-1" &&
            b.IsFake()
            && b.IsEnabled()
            && b.IsInBeta()));

        await bankRepository.Received(1).AddAsync(Arg.Is<Bank>(b =>
            b.LastBanksSynchronisationProcessId == bankSynchronisationProcessId &&
            b.ExternalId == "external-id-2" &&
            b.IsFake()
            && b.IsEnabled() == false));
    }

    [Test]
    public async Task SynchroniseAsync_ForEachExternalProvider_FromProvidedDate_WillCreateANewBankOrUpdateExistingBank()
    {
        var bankSynchronisationProcessId = new BanksSynchronisationProcessId(Guid.NewGuid());
        var fromDate = DateTime.UtcNow;
        var externalProviders = GetExternalProviders();

        var integrationService = Substitute.For<ISaltEdgeIntegrationService>();
        integrationService.FetchProvidersAsync(fromDate).Returns(externalProviders);

        var bankRepository = Substitute.For<IBankRepository>();
        bankRepository.GetByExternalIdAsync("external-id-2").Returns(Bank.AddNew(new BanksSynchronisationProcessId(
            Guid.NewGuid()),
            ExternalProviderName.SaltEdge,
            "external-id-2",
            "Bank name 2",
            Country.FakeCountry,
            BankStatus.Beta,
            true,
            null,
            "https://cdn.savify.localhost/logos/banks/bank-2.png"));

        var service = new BanksSynchronisationService(integrationService, bankRepository);

        await service.SynchroniseAsync(bankSynchronisationProcessId, fromDate);

        await integrationService.Received(1).FetchProvidersAsync(fromDate);
        await bankRepository.Received(1).AddAsync(Arg.Is<Bank>(b =>
            b.LastBanksSynchronisationProcessId == bankSynchronisationProcessId &&
            b.ExternalId == "external-id-1" &&
            b.IsFake()
            && b.IsEnabled()
            && b.IsInBeta()));

        await bankRepository.DidNotReceive().AddAsync(Arg.Is<Bank>(b => b.ExternalId == "external-id-2"));
    }

    private List<SaltEdgeProvider> GetExternalProviders()
    {
        var providers = new List<SaltEdgeProvider>();

        providers.Add(new SaltEdgeProvider(
            "external-id-1",
            "Bank name 1",
            "active",
            "XF",
            true,
            60,
            "https://cdn.savify.localhost/logos/banks/bank-1.png"));

        providers.Add(new SaltEdgeProvider(
            "external-id-2",
            "Bank name 2",
            "inactive",
            "XF",
            false,
            null,
            "https://cdn.savify.localhost/logos/banks/bank-2.png"));

        return providers;
    }
}
