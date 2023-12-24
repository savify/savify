using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Infrastructure.Integrations.Exceptions;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing.Services;

public class BankConnectionProcessInitiationService(
    ISaltEdgeCustomerRepository customerRepository,
    ISaltEdgeIntegrationService saltEdgeIntegrationService)
    : IBankConnectionProcessInitiationService
{
    public async Task InitiateForAsync(UserId userId)
    {
        var customer = await customerRepository.GetOrDefaultAsync(userId.Value);

        if (customer is null)
        {
            try
            {
                var responseContent = await saltEdgeIntegrationService.CreateCustomerAsync(userId.Value);
                await customerRepository.AddAsync(new SaltEdgeCustomer(
                    responseContent.Id,
                    Guid.Parse(responseContent.Identifier)));
            }
            catch (SaltEdgeIntegrationException)
            {
                throw new ExternalProviderException("Something went wrong during bank connection initialization process");
            }
        }
    }
}
