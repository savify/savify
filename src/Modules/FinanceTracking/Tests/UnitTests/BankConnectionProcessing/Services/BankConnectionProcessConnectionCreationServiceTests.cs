using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.Events;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Accounts;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ResponseContent;

namespace App.Modules.FinanceTracking.UnitTests.BankConnectionProcessing.Services;

[TestFixture]
public class BankConnectionProcessConnectionCreationServiceTests : UnitTestBase
{
    [Test]
    public async Task CreatingConnection_IsSuccessful()
    {
        var bankConnectionProcessId = new BankConnectionProcessId(Guid.NewGuid());
        var userId = new UserId(Guid.NewGuid());
        var bankId = new BankId(Guid.NewGuid());
        var externalConnectionId = "123456";

        var saltEdgeConnectionRepository = Substitute.For<ISaltEdgeConnectionRepository>();
        var bankConnectionRepository = Substitute.For<IBankConnectionRepository>();
        var integrationService = Substitute.For<ISaltEdgeIntegrationService>();

        var externalConnection = new SaltEdgeConnection(
            externalConnectionId,
            bankConnectionProcessId.Value,
            "provider_code",
            "XF",
            "123456",
            "123456",
            "active");
        var consent = new SaltEdgeConsent
        {
            Id = "123456",
            ExpiresAt = null
        };

        var saltEdgeAccounts = new List<SaltEdgeAccount>
        {
            new("123456", "Test account", "account", 100.96, "USD")
        };

        integrationService.FetchConnectionAsync(externalConnectionId).Returns(externalConnection);
        integrationService.FetchConsentAsync(externalConnection.LastConsentId, externalConnection.Id).Returns(consent);
        integrationService.FetchAccountsAsync(externalConnection.Id).Returns(saltEdgeAccounts);

        var connectionCreationService = new BankConnectionProcessConnectionCreationService(
            saltEdgeConnectionRepository,
            bankConnectionRepository,
            integrationService);

        var connectionResult = await connectionCreationService.CreateConnection(bankConnectionProcessId, userId, bankId, externalConnectionId);

        Assert.That(connectionResult.IsSuccess, Is.True);
        var connection = connectionResult.Success;

        var connectionCreatedDomainEvent = AssertPublishedDomainEvent<BankConnectionCreatedDomainEvent>(connection);
        Assert.That(connection.Id.Value, Is.EqualTo(bankConnectionProcessId.Value));
        Assert.That(connection.HasMultipleBankAccounts, Is.False);
        Assert.That(connectionCreatedDomainEvent.BankConnectionId.Value, Is.EqualTo(bankConnectionProcessId.Value));
        Assert.That(connectionCreatedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(connectionCreatedDomainEvent.BankId, Is.EqualTo(bankId));

        await integrationService.Received(1).FetchConnectionAsync(externalConnectionId);
        await integrationService.Received(1).FetchConsentAsync(externalConnection.LastConsentId, externalConnection.Id);
        await integrationService.Received(1).FetchAccountsAsync(externalConnection.Id);
        await saltEdgeConnectionRepository.Received(1).AddAsync(Arg.Is<SaltEdgeConnection>(c =>
            c.Id == externalConnectionId && c.InternalConnectionId == bankConnectionProcessId.Value));
        await bankConnectionRepository.Received(1).AddAsync(Arg.Is<BankConnection>(c =>
            c.Id == connection.Id));
    }
}
