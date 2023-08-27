using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;

public class BankConnectionProcessRedirectionService : IBankConnectionProcessRedirectionService
{
    private readonly SaltEdgeCustomerRepository _customerRepository;

    private readonly SaltEdgeIntegrationService _saltEdgeIntegrationService;

    public BankConnectionProcessRedirectionService(
        SaltEdgeCustomerRepository customerRepository,
        SaltEdgeIntegrationService saltEdgeIntegrationService)
    {
        _customerRepository = customerRepository;
        _saltEdgeIntegrationService = saltEdgeIntegrationService;
    }

    public async Task<Redirection> Redirect(UserId userId, BankId bankId)
    {
        var customer = await _customerRepository.GetSaltEdgeCustomerForAsync(userId.Value);
        var providerCode = "fakebank_interactive_xf"; // TODO: get provider code (external bank id) from 'Banks' module
        var returnToUrl = "https://display-parameters.com/"; // TODO: get url from configuration

        // TODO: handle different locales (languages) at CreateConnectSessionRequestContent.Attempt (get language from User)
        var responseContent = await _saltEdgeIntegrationService.CreateConnectSession(customer.Id, providerCode, returnToUrl);

        return new Redirection(responseContent.ConnectUrl, responseContent.ExpiresAt);
    }
}
