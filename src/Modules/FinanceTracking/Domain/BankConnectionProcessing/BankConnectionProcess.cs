using App.BuildingBlocks.Domain;
using App.BuildingBlocks.Domain.Results;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Rules;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing;

public class BankConnectionProcess : Entity, IAggregateRoot
{
    public BankConnectionProcessId Id { get; private set; }

    public UserId UserId { get; set; }

    internal BankId BankId { get; set; }

    internal WalletId WalletId { get; set; }

    private WalletType _walletType;

    private BankConnectionProcessStatus _status;

    private string? _redirectUrl;

    private DateTime _initiatedAt;

    private DateTime? _redirectUrlExpiresAt;

    private DateTime? _updatedAt;

    public static async Task<BankConnectionProcess> Initiate(UserId userId, BankId bankId, WalletId walletId, WalletType walletType, IBankConnectionProcessInitiationService initiationService)
    {
        await initiationService.InitiateForAsync(userId);

        return new BankConnectionProcess(userId, bankId, walletId, walletType);
    }

    public async Task<Result<string, RedirectionError>> Redirect(IBankConnectionProcessRedirectionService redirectionService)
    {
        CheckRules(new BankConnectionProcessCannotMakeRedirectionWhenRedirectUrlIsExpiredRule(_status),
            new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status));

        var redirectionResult = await redirectionService.Redirect(Id, UserId, BankId);

        if (redirectionResult.IsError && redirectionResult.Error == RedirectionError.ExternalProviderError)
        {
            _status = _status.ToErrorAtProvider();
            _updatedAt = DateTime.UtcNow;

            return redirectionResult.Error;
        }


        var redirection = redirectionResult.Success;
        _redirectUrl = redirection.Url;
        _redirectUrlExpiresAt = redirection.ExpiresAt;
        _status = _status.ToRedirected();
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserRedirectedDomainEvent(Id, _redirectUrlExpiresAt.Value));

        return _redirectUrl;
    }

    public async Task<Result<BankConnection, CreateConnectionError>> CreateConnection(
        string externalConnectionId,
        IBankConnectionProcessConnectionCreationService connectionCreationService,
        IBankAccountConnector bankAccountConnector)
    {
        CheckRules(new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status));

        var connectionResult = await connectionCreationService.CreateConnection(Id, UserId, BankId, externalConnectionId);

        if (connectionResult.IsError && connectionResult.Error == CreateConnectionError.ExternalProviderError)
        {
            _status = _status.ToErrorAtProvider();
            _updatedAt = DateTime.UtcNow;

            return connectionResult.Error;
        }

        var connection = connectionResult.Success;

        if (connection.HasMultipleBankAccounts())
        {
            _status = _status.ToWaitingForAccountChoosing();
        }
        else
        {
            var bankAccountId = connection.GetSingleBankAccount().Id;
            await bankAccountConnector.ConnectBankAccountToWallet(WalletId, _walletType, new BankConnectionId(Id.Value), bankAccountId);

            _status = _status.ToCompleted();

            AddDomainEvent(new BankConnectionProcessCompletedDomainEvent(Id, bankAccountId));
        }

        _updatedAt = DateTime.UtcNow;

        return connection;
    }

    public async Task ChooseBankAccount(BankAccountId bankAccountId, IBankAccountConnector bankAccountConnector)
    {
        CheckRules(new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status));

        await bankAccountConnector.ConnectBankAccountToWallet(WalletId, _walletType, new BankConnectionId(Id.Value), bankAccountId);

        _status = _status.ToCompleted();
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankConnectionProcessCompletedDomainEvent(Id, bankAccountId));
    }

    public void Expire()
    {
        CheckRules(new RedirectUrlShouldBeExpiredRule(_redirectUrlExpiresAt));

        _status = _status.ToRedirectUrlExpired();
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankConnectionProcessExpiredDomainEvent(Id));
    }

    private BankConnectionProcess(UserId userId, BankId bankId, WalletId walletId, WalletType walletType)
    {
        Id = new BankConnectionProcessId(Guid.NewGuid());
        UserId = userId;
        BankId = bankId;
        WalletId = walletId;
        _walletType = walletType;
        _status = BankConnectionProcessStatus.Initiated;
        _initiatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankConnectionProcessInitiatedDomainEvent(Id, UserId, BankId, WalletId));
    }

    private BankConnectionProcess() { }
}
