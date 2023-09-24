using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.BankConnections.Events;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Accounts;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent;

namespace App.Modules.Wallets.UnitTests.BankConnectionProcessing.Services;

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
        var consent = new SaltEdgeConsent();
        consent.Id = "123456";
        consent.ExpiresAt = null;

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

        var connection = await connectionCreationService.CreateConnection(bankConnectionProcessId, userId, bankId, externalConnectionId);

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
