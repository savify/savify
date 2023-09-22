using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent;

namespace App.Modules.Wallets.UnitTests.BankConnectionProcessing.Services;

[TestFixture]
public class BankConnectionProcessInitiationServiceTests : UnitTestBase
{
    [Test]
    public async Task InitiationForUser_ThatDoesNotHaveExternalCustomer_CreatesCustomer()
    {
        var userId = new UserId(Guid.NewGuid());

        var customerRepository = Substitute.For<ISaltEdgeCustomerRepository>();

        var integrationService = Substitute.For<ISaltEdgeIntegrationService>();
        var createCustomerResponseContent = new CreateCustomerResponseContent();
        createCustomerResponseContent.Id = "123456";
        createCustomerResponseContent.Identifier = userId.Value.ToString();

        integrationService.CreateCustomerAsync(userId.Value).Returns(Task.FromResult(createCustomerResponseContent));

        var customer = new SaltEdgeCustomer(
            createCustomerResponseContent.Id,
            Guid.Parse(createCustomerResponseContent.Identifier));
        customerRepository.AddAsync(customer).Returns(Task.CompletedTask);

        var service = new BankConnectionProcessInitiationService(customerRepository, integrationService);

        await service.InitiateForAsync(userId);

        await customerRepository.Received(1).GetOrDefaultAsync(userId.Value);
        await customerRepository.Received(1).AddAsync(Arg.Is<SaltEdgeCustomer>(c =>
            c.Id == customer.Id && c.Identifier == customer.Identifier));
        await integrationService.Received(1).CreateCustomerAsync(userId.Value);
    }

    [Test]
    public async Task InitiationForUser_ThatHasExternalCustomer_WillDoNothing()
    {
        var userId = new UserId(Guid.NewGuid());

        var customerRepository = Substitute.For<ISaltEdgeCustomerRepository>();
        var integrationService = Substitute.For<ISaltEdgeIntegrationService>();

        customerRepository.GetOrDefaultAsync(userId.Value).Returns(new SaltEdgeCustomer("123456", Guid.NewGuid()));

        var service = new BankConnectionProcessInitiationService(customerRepository, integrationService);

        await service.InitiateForAsync(userId);

        await customerRepository.Received(1).GetOrDefaultAsync(userId.Value);
        await customerRepository.Received(0).AddAsync(Arg.Any<SaltEdgeCustomer>());
        await integrationService.Received(0).CreateCustomerAsync(userId.Value);
    }
}
