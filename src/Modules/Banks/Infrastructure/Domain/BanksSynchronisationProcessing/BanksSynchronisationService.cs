using App.BuildingBlocks.Domain.Results;
using App.Modules.Banks.Domain;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;
using App.Modules.Banks.Domain.ExternalProviders;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge;
using App.Modules.Banks.Infrastructure.Integrations.SaltEdge.Providers;

namespace App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;

public class BanksSynchronisationService(
    ISaltEdgeIntegrationService saltEdgeIntegrationService,
    IBankRepository bankRepository)
    : IBanksSynchronisationService
{
    public async Task<Result> SynchroniseAsync(BanksSynchronisationProcessId banksSynchronisationProcessId)
    {
        var banks = await bankRepository.GetAllAsync();

        List<SaltEdgeProvider> externalProviders;
        try
        {
            externalProviders = await saltEdgeIntegrationService.FetchProvidersAsync();
        }
        catch (SaltEdgeIntegrationException)
        {
            return Result.Error;
        }

        foreach (var bank in banks)
        {
            if (externalProviders.All(p => p.Code != bank.ExternalId))
            {
                bank.Disable();
            }
        }

        foreach (var externalProvider in externalProviders)
        {
            var bank = banks.FirstOrDefault(b => b.ExternalId == externalProvider.Code);

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

                await bankRepository.AddAsync(bank);
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

        return Result.Success;
    }
}
