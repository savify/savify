using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Events;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.BankAccountConnections;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing;

public class BankConnectionProcess : Entity, IAggregateRoot
{
    public BankConnectionProcessId Id { get; private set; }

    public UserId UserId { get; set; }

    internal BankId BankId { get; set; }

    internal WalletId WalletId { get; set; }

    private WalletType _walletType;

    private BankConnectionProcessStatus _status;

    private string? _redirectUrl = null;

    private DateTime _initiatedAt;

    private DateTime? _redirectUrlExpiresAt = null;

    private DateTime? _updatedAt = null;

    public static async Task<BankConnectionProcess> Initiate(UserId userId, BankId bankId, WalletId walletId, WalletType walletType, IBankConnectionProcessInitiationService initiationService)
    {
        await initiationService.InitiateForAsync(userId);

        return new BankConnectionProcess(userId, bankId, walletId, walletType);
    }

    public async Task<string> Redirect(IBankConnectionProcessRedirectionService redirectionService)
    {
        CheckRules(new BankConnectionProcessCannotMakeRedirectionWhenRedirectUrlIsExpiredRule(_status),
            new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status),
            new BankConnectionProcessShouldKeepValidStatusTransitionsRule(_status, BankConnectionProcessStatus.Redirected));

        try
        {
            var redirection = await redirectionService.Redirect(Id, UserId, BankId);

            _redirectUrl = redirection.Url;
            _redirectUrlExpiresAt = redirection.ExpiresAt;
            _status = BankConnectionProcessStatus.Redirected;
            _updatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserRedirectedDomainEvent(Id, _redirectUrlExpiresAt.Value));

            return _redirectUrl;
        }
        catch (DomainException)
        {
            _status = BankConnectionProcessStatus.ErrorAtProvider;
            _updatedAt = DateTime.UtcNow;

            // TODO: if we throw exception data will not be saved. We should return some kind of result
            throw;
        }
    }

    public async Task<BankConnection> CreateConnection(
        string externalConnectionId,
        IBankConnectionProcessConnectionCreationService connectionCreationService,
        IBankAccountConnector bankAccountConnector)
    {
        CheckRules(new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status));

        try
        {
            var connection = await connectionCreationService.CreateConnection(Id, UserId, BankId, externalConnectionId);

            if (connection.HasMultipleBankAccounts())
            {
                CheckRules(new BankConnectionProcessShouldKeepValidStatusTransitionsRule(_status, BankConnectionProcessStatus.WaitingForAccountChoosing));
                _status = BankConnectionProcessStatus.WaitingForAccountChoosing;
            }
            else
            {
                CheckRules(new BankConnectionProcessShouldKeepValidStatusTransitionsRule(_status, BankConnectionProcessStatus.Completed));

                var bankAccountId = connection.GetSingleBankAccount().Id;
                await bankAccountConnector.ConnectBankAccountToWallet(WalletId, _walletType, new BankConnectionId(Id.Value), bankAccountId);

                _status = BankConnectionProcessStatus.Completed;

                AddDomainEvent(new BankConnectionProcessCompletedDomainEvent(Id, bankAccountId));
            }

            _updatedAt = DateTime.UtcNow;

            return connection;
        }
        catch (DomainException)
        {
            _status = BankConnectionProcessStatus.ErrorAtProvider;
            _updatedAt = DateTime.UtcNow;

            // TODO: if we throw exception data will not be saved. We should return some kind of result
            throw;
        }
    }

    public async Task ChooseBankAccount(BankAccountId bankAccountId, IBankAccountConnector bankAccountConnector)
    {
        CheckRules(new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status),
            new BankConnectionProcessShouldKeepValidStatusTransitionsRule(_status, BankConnectionProcessStatus.Completed));

        await bankAccountConnector.ConnectBankAccountToWallet(WalletId, _walletType, new BankConnectionId(Id.Value), bankAccountId);

        _status = BankConnectionProcessStatus.Completed;
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankConnectionProcessCompletedDomainEvent(Id, bankAccountId));
    }

    public void Expire()
    {
        CheckRules(new RedirectUrlShouldBeExpiredRule(_redirectUrlExpiresAt),
            new BankConnectionProcessShouldKeepValidStatusTransitionsRule(_status, BankConnectionProcessStatus.RedirectUrlExpired));

        _status = BankConnectionProcessStatus.RedirectUrlExpired;
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
