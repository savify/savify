using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;
using App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnectionProcessing.Services;

public class BankConnectionProcessConnectionCreationService(
    ISaltEdgeConnectionRepository connectionRepository,
    IBankConnectionRepository bankConnectionRepository,
    ISaltEdgeIntegrationService saltEdgeIntegrationService)
    : IBankConnectionProcessConnectionCreationService
{
    public async Task<Result<BankConnection, CreateConnectionError>> CreateConnection(BankConnectionProcessId id, UserId userId, BankId bankId, string externalConnectionId)
    {
        try
        {
            var saltEdgeConnection = await saltEdgeIntegrationService.FetchConnectionAsync(externalConnectionId);
            var saltEdgeConsent = await saltEdgeIntegrationService.FetchConsentAsync(saltEdgeConnection.LastConsentId, saltEdgeConnection.Id);
            var saltEdgeAccounts = await saltEdgeIntegrationService.FetchAccountsAsync(saltEdgeConnection.Id);

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

            await connectionRepository.AddAsync(saltEdgeConnection);
            await bankConnectionRepository.AddAsync(connection);

            return connection;
        }
        catch (SaltEdgeIntegrationException)
        {
            return CreateConnectionError.ExternalProviderError;
        }

    }
}
