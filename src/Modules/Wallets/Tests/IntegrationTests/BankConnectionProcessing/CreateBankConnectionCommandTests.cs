using App.Modules.Wallets.Application.BankConnectionProcessing.CreateBankConnection;
using App.Modules.Wallets.Application.BankConnectionProcessing.GetBankConnectionProcess;
using App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.Wallets.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.Wallets.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.IntegrationTests.SeedData;
using App.Modules.Wallets.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.Wallets.IntegrationTests.BankConnectionProcessing;

[TestFixture]
public class CreateBankConnectionCommandTests : TestBase
{
    private Guid _walletId;

    [Test]
    public async Task CreateBankConnectionCommand_WithSingleAccount_FinishesBankConnectionProcess_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchConnectionSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockFetchConsentSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockFetchAccountsSuccessfulResponse(hasMultipleAccounts: false);

        var bankConnectionProcessId = await InitiateBankConnectionProcess();

        await WalletsModule.ExecuteCommandAsync(new CreateBankConnectionCommand(bankConnectionProcessId, BankConnectionProcessingData.ExternalConnectionId));

        var bankConnectionProcess = await WalletsModule.ExecuteQueryAsync(new GetBankConnectionProcessQuery(bankConnectionProcessId));
        var wallet = await WalletsModule.ExecuteQueryAsync(new GetDebitWalletQuery(_walletId));

        Assert.That(bankConnectionProcess, Is.Not.Null);
        Assert.That(bankConnectionProcess.Status, Is.EqualTo(BankConnectionProcessStatus.State.Completed.ToString()));
        Assert.That(await CountBankAccountsInConnection(bankConnectionProcess.Id), Is.EqualTo(1));
        Assert.That(wallet.Balance, Is.EqualTo((int)BankConnectionProcessingData.ExternalUSDAccountBalance * 100));
        Assert.That(wallet.Currency, Is.EqualTo(BankConnectionProcessingData.ExternalUSDAccountCurrency));
        Assert.That(wallet.BankConnectionId, Is.EqualTo(bankConnectionProcess.Id));
        Assert.That(wallet.BankAccountId, Is.Not.Null);
    }

    [Test]
    public async Task CreateBankConnectionCommand_WithMultipleAccounts_FinishesBankConnectionProcess_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchConnectionSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockFetchConsentSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockFetchAccountsSuccessfulResponse(hasMultipleAccounts: true);

        var bankConnectionProcessId = await InitiateBankConnectionProcess();

        await WalletsModule.ExecuteCommandAsync(new CreateBankConnectionCommand(bankConnectionProcessId, BankConnectionProcessingData.ExternalConnectionId));

        var bankConnectionProcess = await WalletsModule.ExecuteQueryAsync(new GetBankConnectionProcessQuery(bankConnectionProcessId));

        Assert.That(bankConnectionProcess, Is.Not.Null);
        Assert.That(bankConnectionProcess.Status, Is.EqualTo(BankConnectionProcessStatus.State.WaitingForAccountChoosing.ToString()));
        Assert.That(await CountBankAccountsInConnection(bankConnectionProcess.Id), Is.EqualTo(2));
    }

    private async Task<Guid> InitiateBankConnectionProcess()
    {
        SaltEdgeHttpClientMocker.MockCreateCustomerSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockCreateConnectSessionSuccessfulResponse();

        _walletId = await AddDebitWalletFor(BankConnectionProcessingData.UserId);
        var result = await WalletsModule.ExecuteCommandAsync(new ConnectBankAccountToDebitWalletCommand(
            BankConnectionProcessingData.UserId,
            _walletId,
            BankConnectionProcessingData.BankId));

        return result.Success.Id;
    }

    private async Task<int> CountBankAccountsInConnection(Guid bankConnectionId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = @"SELECT COUNT(*) FROM wallets.bank_accounts a WHERE a.bank_connection_id = @ConnectionId";

        return await connection.QuerySingleAsync<int>(sql, new { ConnectionId = bankConnectionId });
    }

    private async Task<Guid> AddDebitWalletFor(Guid userId)
    {
        var command = new AddNewDebitWalletCommand(
            userId,
            "Debit wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        return await WalletsModule.ExecuteCommandAsync(command);
    }
}
