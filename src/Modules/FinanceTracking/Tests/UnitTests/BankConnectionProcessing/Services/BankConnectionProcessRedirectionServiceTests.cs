using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Customers;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ResponseContent;

namespace App.Modules.FinanceTracking.UnitTests.BankConnectionProcessing.Services;

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
        customerRepository.GetAsync(userId.Value).Returns(new SaltEdgeCustomer("123456", userId.Value));

        var responseContent = new CreateConnectSessionResponseContent
        {
            ConnectUrl = "https://connect-url.com/connect",
            ExpiresAt = DateTime.UtcNow
        };

        var integrationService = Substitute.For<ISaltEdgeIntegrationService>();
        // TODO: logic will be changed for provider code and return to url
        integrationService.CreateConnectSessionAsync(
            bankConnectionProcessId.Value,
            "123456",
            "fakebank_interactive_xf",
            "https://display-parameters.com/")
            .Returns(responseContent);

        var redirectionService = new BankConnectionProcessRedirectionService(customerRepository, integrationService);

        var redirectionResult = await redirectionService.Redirect(bankConnectionProcessId, userId, bankId);

        Assert.That(redirectionResult.Success.Url, Is.EqualTo(responseContent.ConnectUrl));
        Assert.That(redirectionResult.Success.ExpiresAt, Is.EqualTo(responseContent.ExpiresAt));

        await customerRepository.Received(1).GetAsync(userId.Value);
        await integrationService.Received(1).CreateConnectSessionAsync(bankConnectionProcessId.Value,
            "123456",
            "fakebank_interactive_xf",
            "https://display-parameters.com/");
    }
}
