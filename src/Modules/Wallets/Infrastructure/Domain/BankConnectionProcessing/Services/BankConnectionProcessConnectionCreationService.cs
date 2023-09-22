using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;
using App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing.Services;

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

    public async Task<BankConnection> CreateConnection(BankConnectionProcessId id, UserId userId, BankId bankId, string externalConnectionId)
    {
        try
        {
            var saltEdgeConnection = await _saltEdgeIntegrationService.FetchConnectionAsync(externalConnectionId);
            //The type of saltEdgeConsent is Consent. Most of entities in salt edge integration contain SaltEdge prefix.
            //Should we refactor it then?
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
            //Are we sure we are going to throw a domain exception here? Maybe it had to be called 'InfrastructureException'?
            //It looks like we are doing a domain job here. If the job was not done we throw something is wrong with a domain.
            //Here for example the TPP could be offline for a while. But it's not our domain problem, it's external's service problem.
            throw new DomainException("Something went wrong during bank connection processing. Try again or contact support.");
        }

    }
}
