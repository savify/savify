using App.Modules.Wallets.Domain;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;

public class BankConnectionProcessConnectionCreationService : IBankConnectionProcessConnectionCreationService
{
    private readonly SaltEdgeConnectionRepository _connectionRepository;

    private readonly SaltEdgeIntegrationService _saltEdgeIntegrationService;

    public BankConnectionProcessConnectionCreationService(SaltEdgeConnectionRepository connectionRepository, SaltEdgeIntegrationService saltEdgeIntegrationService)
    {
        _connectionRepository = connectionRepository;
        _saltEdgeIntegrationService = saltEdgeIntegrationService;
    }

    public async Task<BankConnection> CreateConnection(BankConnectionProcessId id, UserId userId, BankId bankId, string externalConnectionId)
    {
        var saltEdgeConnection = await _saltEdgeIntegrationService.FetchConnection(externalConnectionId);
        var saltEdgeConsent = await _saltEdgeIntegrationService.FetchConsent(saltEdgeConnection.LastConsentId, saltEdgeConnection.Id);
        var saltEdgeAccounts = await _saltEdgeIntegrationService.FetchAccounts(saltEdgeConnection.Id);

        var connection = BankConnection.CreateFromBankConnectionProcess(id, bankId, userId, new Consent(saltEdgeConsent.ExpiresAt ?? DateTime.MaxValue));

        saltEdgeConnection.InternalConnectionId = connection.Id.Value;
        saltEdgeAccounts.ForEach(account => connection.AddBankAccount(
            account.Id,
            account.Name,
            (int)(account.Balance * 100.00),
            new Currency(account.CurrencyCode)));

        await _connectionRepository.AddAsync(saltEdgeConnection);

        return connection;
    }
}
