using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;

public class BankConnectionProcessInitiationService : IBankConnectionProcessInitiationService
{
    private readonly SaltEdgeCustomerRepository _customerRepository;

    private readonly SaltEdgeIntegrationService _saltEdgeIntegrationService;

    public BankConnectionProcessInitiationService(SaltEdgeCustomerRepository customerRepository, SaltEdgeIntegrationService saltEdgeIntegrationService)
    {
        _customerRepository = customerRepository;
        _saltEdgeIntegrationService = saltEdgeIntegrationService;
    }

    public async Task InitiateForAsync(UserId userId)
    {
        var customer = await _customerRepository.GetSaltEdgeCustomerForAsync(userId.Value);

        if (customer is null)
        {
            var responseContent = await _saltEdgeIntegrationService.CreateCustomer(userId.Value);
            await _customerRepository.AddAsync(new SaltEdgeCustomer(
                responseContent.Id,
                Guid.Parse(responseContent.Identifier)));
        }
    }
}
