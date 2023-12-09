using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing.Services;

public class BankConnectionProcessConnectionCreationService : IBankConnectionProcessConnectionCreationService
{
    private readonly ISaltEdgeConnectionRepository _connectionRepository;

    private readonly IBankConnectionRepository _bankConnectionRepository;

    private readonly ISaltEdgeIntegrationService _saltEdgeIntegrationService;

    public BankConnectionProcessConnectionCreationService(
        ISaltEdgeConnectionRepository connectionRepository,
        IBankConnectionRepository bankConnectionRepository,
        ISaltEdgeIntegrationService saltEdgeIntegrationService)
    {
        _connectionRepository = connectionRepository;
        _bankConnectionRepository = bankConnectionRepository;
        _saltEdgeIntegrationService = saltEdgeIntegrationService;
    }

    public async Task<Result<BankConnection, CreateConnectionError>> CreateConnection(BankConnectionProcessId id, UserId userId, BankId bankId, string externalConnectionId)
    {
        try
        {
            var saltEdgeConnection = await _saltEdgeIntegrationService.FetchConnectionAsync(externalConnectionId);
            var saltEdgeConsent = await _saltEdgeIntegrationService.FetchConsentAsync(saltEdgeConnection.LastConsentId, saltEdgeConnection.Id);
            var saltEdgeAccounts = await _saltEdgeIntegrationService.FetchAccountsAsync(saltEdgeConnection.Id);

            var connection = BankConnection.CreateFromBankConnectionProcess(
                id,
                bankId,
                userId,
                new Consent(saltEdgeConsent.ExpiresAt?.ToUniversalTime() ?? DateTime.MaxValue.ToUniversalTime()));

            saltEdgeConnection.InternalConnectionId = connection.Id.Value;
            saltEdgeAccounts.ForEach(account => connection.AddBankAccount(
                account.Id,
                account.Name,
                (int)(account.Balance * 100.00),
                new Currency(account.CurrencyCode)));

            await _connectionRepository.AddAsync(saltEdgeConnection);
            await _bankConnectionRepository.AddAsync(connection);

            return connection;
        }
        catch (SaltEdgeIntegrationException)
        {
            return CreateConnectionError.ExternalProviderError;
        }

    }
}
