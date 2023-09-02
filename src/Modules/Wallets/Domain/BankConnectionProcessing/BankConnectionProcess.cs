using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Events;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.BankAccountConnection;

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

    // TODO: rename to _redirectUrlExpiresAt
    private DateTime? _expiresAt = null;

    private DateTime? _updatedAt = null;

    public static async Task<BankConnectionProcess> Initiate(UserId userId, BankId bankId, WalletId walletId, WalletType walletType, IBankConnectionProcessInitiationService initiationService)
    {
        await initiationService.InitiateForAsync(userId);

        return new BankConnectionProcess(userId, bankId, walletId, walletType);
    }

    public async Task<string> Redirect(IBankConnectionProcessRedirectionService redirectionService)
    {
        // TODO: check status transition rule
        // TODO: check expiration rule
        CheckRules(new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status));

        var redirection = await redirectionService.Redirect(Id, UserId, BankId);

        _redirectUrl = redirection.Url;
        _expiresAt = redirection.ExpiresAt;
        _status = BankConnectionProcessStatus.Redirected;
        _updatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserRedirectedDomainEvent(Id, (DateTime)_expiresAt));

        return _redirectUrl;
    }

    public async Task<BankConnection> CreateConnection(
        string externalConnectionId,
        IBankConnectionProcessConnectionCreationService connectionCreationService,
        BankAccountConnector bankAccountConnector)
    {
        // TODO: check status transition rule
        CheckRules(new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status));

        var connection = await connectionCreationService.CreateConnection(Id, UserId, BankId, externalConnectionId);

        if (connection.HasMultipleBankAccounts())
        {
            _status = BankConnectionProcessStatus.WaitingForAccountChoosing;
        }
        else
        {
            var bankAccountId = connection.GetSingleBankAccount().Id;
            await bankAccountConnector.ConnectBankAccountToWallet(WalletId, _walletType, new BankConnectionId(Id.Value), bankAccountId);

            _status = BankConnectionProcessStatus.Completed;
        }

        _updatedAt = DateTime.UtcNow;

        return connection;
    }

    public async Task ChooseBankAccount(BankAccountId bankAccountId, BankAccountConnector bankAccountConnector)
    {
        // TODO: check status transition rule
        CheckRules(new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status));

        await bankAccountConnector.ConnectBankAccountToWallet(WalletId, _walletType, new BankConnectionId(Id.Value), bankAccountId);

        _status = BankConnectionProcessStatus.Completed;
        _updatedAt = DateTime.UtcNow;
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

        AddDomainEvent(new BankConnectionProcessInitiatedDomainEvent(Id, UserId, BankId));
    }

    private BankConnectionProcess() { }
}
