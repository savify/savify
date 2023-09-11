using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Customers;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent;

namespace App.Modules.Wallets.UnitTests.BankConnectionProcessing.Services;

[TestFixture]
public class BankConnectionProcessRedirectionServiceTests : UnitTestBase
{
    [Test]
    public async Task Redirection_IsSuccessful_AndReturnsRedirectionUrl()
    {
        var bankConnectionProcessId = new BankConnectionProcessId(Guid.NewGuid());
        var userId = new UserId(Guid.NewGuid());
        var bankId = new BankId(Guid.NewGuid());

        var customerRepository = Substitute.For<ISaltEdgeCustomerRepository>();
        customerRepository.GetSaltEdgeCustomerOrDefaultAsync(userId.Value).Returns(new SaltEdgeCustomer("123456", userId.Value));

        var responseContent = new CreateConnectSessionResponseContent();
        responseContent.ConnectUrl = "https://connect-url.com/connect";
        responseContent.ExpiresAt = DateTime.UtcNow;

        var integrationService = Substitute.For<ISaltEdgeIntegrationService>();
        // TODO: logic will be changed for provider code and return to url
        integrationService.CreateConnectSessionAsync(
            bankConnectionProcessId.Value,
            "123456",
            "fakebank_interactive_xf",
            "https://display-parameters.com/")
            .Returns(responseContent);

        var redirectionService = new BankConnectionProcessRedirectionService(customerRepository, integrationService);

        var redirection = await redirectionService.Redirect(bankConnectionProcessId, userId, bankId);

        Assert.That(redirection.Url, Is.EqualTo(responseContent.ConnectUrl));
        Assert.That(redirection.ExpiresAt, Is.EqualTo(responseContent.ExpiresAt));

        await customerRepository.Received(1).GetSaltEdgeCustomerOrDefaultAsync(userId.Value);
        await integrationService.Received(1).CreateConnectSessionAsync(bankConnectionProcessId.Value,
            "123456",
            "fakebank_interactive_xf",
            "https://display-parameters.com/");
    }
}
