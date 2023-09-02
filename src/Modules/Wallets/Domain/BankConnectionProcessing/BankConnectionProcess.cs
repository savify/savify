using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Events;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing;

public class BankConnectionProcess : Entity, IAggregateRoot
{
    public BankConnectionProcessId Id { get; private set; }

    internal UserId UserId { get; set; }

    internal BankId BankId { get; set; }

    internal WalletId WalletId { get; set; }

    private BankConnectionProcessStatus _status;

    private string? _redirectUrl = null;

    private DateTime _initiatedAt;

    private DateTime? _expiresAt = null;

    private DateTime? _updatedAt = null;

    public static async Task<BankConnectionProcess> Initiate(UserId userId, BankId bankId, WalletId walletId, IBankConnectionProcessInitiationService initiationService)
    {
        await initiationService.InitiateForAsync(userId);

        return new BankConnectionProcess(userId, bankId, walletId);
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

    public async Task<BankConnection> CreateConnection(string externalConnectionId, IBankConnectionProcessConnectionCreationService connectionCreationService)
    {
        // TODO: check status transition rule
        // TODO: check expiration rule
        CheckRules(new CannotOperateOnBankConnectionProcessWithFinalStatusRule(_status));

        var connection = await connectionCreationService.CreateConnection(Id, UserId, BankId, externalConnectionId);

        if (connection.HasMultipleBankAccounts())
        {
            _status = BankConnectionProcessStatus.WaitingForAccountChoosing;
        }
        else
        {
            // TODO: connect bank account to given wallet
        }

        _updatedAt = DateTime.UtcNow;

        return connection;
    }

    private BankConnectionProcess(UserId userId, BankId bankId, WalletId walletId)
    {
        Id = new BankConnectionProcessId(Guid.NewGuid());
        UserId = userId;
        BankId = bankId;
        WalletId = walletId;
        _status = BankConnectionProcessStatus.Initiated;
        _initiatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankConnectionProcessInitiatedDomainEvent(Id, UserId, BankId));
    }

    private BankConnectionProcess() { }
}
