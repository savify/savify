using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing.Services;

public class BankConnectionProcessRedirectionService(
    ISaltEdgeCustomerRepository customerRepository,
    ISaltEdgeIntegrationService saltEdgeIntegrationService)
    : IBankConnectionProcessRedirectionService
{
    public async Task<Result<Redirection, RedirectionError>> Redirect(BankConnectionProcessId id, UserId userId, BankId bankId, Language language)
    {
        var customer = await customerRepository.GetAsync(userId.Value);
        var providerCode = "fakebank_interactive_xf"; // TODO: get provider code (external bank id) from 'Banks' module
        var returnToUrl = "https://display-parameters.com/"; // TODO: get url from configuration

        try
        {
            // TODO: handle different locales (languages) at CreateConnectSessionRequestContent.Attempt (get language from User)
            var responseContent = await saltEdgeIntegrationService.CreateConnectSessionAsync(id.Value, customer.Id, providerCode, returnToUrl, language.Value);

            return new Redirection(responseContent.ConnectUrl, responseContent.ExpiresAt.ToUniversalTime());
        }
        catch (SaltEdgeIntegrationException)
        {
            return RedirectionError.ExternalProviderError;
        }
    }
}
