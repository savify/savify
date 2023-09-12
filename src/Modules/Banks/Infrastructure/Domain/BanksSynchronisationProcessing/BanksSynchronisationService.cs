using App.Modules.Banks.Domain;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;
using App.Modules.Banks.Domain.ExternalProviders;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge;

namespace App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;

public class BanksSynchronisationService : IBanksSynchronisationService
{
    private readonly ISaltEdgeIntegrationService _saltEdgeIntegrationService;

    private readonly IBankRepository _bankRepository;

    public BanksSynchronisationService(
        ISaltEdgeIntegrationService saltEdgeIntegrationService,
        IBankRepository bankRepository)
    {
        _saltEdgeIntegrationService = saltEdgeIntegrationService;
        _bankRepository = bankRepository;
    }

    public async Task SynchroniseAsync(BanksSynchronisationProcessId banksSynchronisationProcessId)
    {
        var externalProviders = await _saltEdgeIntegrationService.FetchProvidersAsync();

        foreach (var externalProvider in externalProviders)
        {
            var bankStatus = externalProvider.Status is "inactive" or "disabled" ? BankStatus.Disabled : BankStatus.Beta;

            var bank = Bank.AddNew(
                banksSynchronisationProcessId,
                ExternalProviderName.SaltEdge,
                externalProvider.Code,
                externalProvider.Name,
                Country.From(externalProvider.CountryCode),
                bankStatus,
                externalProvider.Regulated,
                externalProvider.MaxConsentDays,
                externalProvider.LogoUrl);

            await _bankRepository.AddAsync(bank);
        }
    }

    public async Task SynchroniseAsync(BanksSynchronisationProcessId banksSynchronisationProcessId, DateTime fromDate)
    {
        var externalProviders = await _saltEdgeIntegrationService.FetchProvidersAsync(fromDate);

        foreach (var externalProvider in externalProviders)
        {
            var bank = await _bankRepository.GetByExternalIdAsync(externalProvider.Code);

            if (bank is null)
            {
                var bankStatus = externalProvider.Status is "inactive" or "disabled" ? BankStatus.Disabled : BankStatus.Beta;

                bank = Bank.AddNew(
                    banksSynchronisationProcessId,
                    ExternalProviderName.SaltEdge,
                    externalProvider.Code,
                    externalProvider.Name,
                    Country.From(externalProvider.CountryCode),
                    bankStatus,
                    externalProvider.Regulated,
                    externalProvider.MaxConsentDays,
                    externalProvider.LogoUrl);

                await _bankRepository.AddAsync(bank);
            }
            else
            {
                var wasDisabled = externalProvider.Status is "inactive" or "disabled";

                bank.Update(
                    banksSynchronisationProcessId,
                    externalProvider.Name,
                    wasDisabled,
                    externalProvider.Regulated,
                    externalProvider.MaxConsentDays,
                    externalProvider.LogoUrl);
            }
        }
    }
}
