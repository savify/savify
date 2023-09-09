using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;

public class BankConnectionProcessInitiationService : IBankConnectionProcessInitiationService
{
    private readonly ISaltEdgeCustomerRepository _customerRepository;

    private readonly ISaltEdgeIntegrationService _saltEdgeIntegrationService;

    public BankConnectionProcessInitiationService(ISaltEdgeCustomerRepository customerRepository, ISaltEdgeIntegrationService saltEdgeIntegrationService)
    {
        _customerRepository = customerRepository;
        _saltEdgeIntegrationService = saltEdgeIntegrationService;
    }

    public async Task InitiateForAsync(UserId userId)
    {
        var customer = await _customerRepository.GetSaltEdgeCustomerOrDefaultAsync(userId.Value);

        if (customer is null)
        {
            var responseContent = await _saltEdgeIntegrationService.CreateCustomerAsync(userId.Value);
            await _customerRepository.AddAsync(new SaltEdgeCustomer(
                responseContent.Id,
                Guid.Parse(responseContent.Identifier)));
        }
    }
}
