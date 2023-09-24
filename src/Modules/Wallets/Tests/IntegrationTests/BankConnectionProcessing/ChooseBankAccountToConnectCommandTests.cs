using App.Modules.Wallets.Application.BankConnectionProcessing.ChooseBankAccountToConnect;
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
public class ChooseBankAccountToConnectCommandTests : TestBase
{
    private Guid _walletId;

    [Test]
    public async Task ChooseBankAccountToConnectCommand_Test()
    {
        var bankConnectionProcessId = await InitiateBankConnectionProcess();
        await CreateBankConnectionWithMultipleBankAccounts(bankConnectionProcessId);

        var bankAccountId = await GetBankAccountByExternalId(BankConnectionProcessingData.ExternalUSDAccountId);

        await WalletsModule.ExecuteCommandAsync(new ChooseBankAccountToConnectCommand(
            bankConnectionProcessId,
            BankConnectionProcessingData.UserId,
            bankAccountId
        ));

        var bankConnectionProcess = await WalletsModule.ExecuteQueryAsync(new GetBankConnectionProcessQuery(bankConnectionProcessId));
        var wallet = await WalletsModule.ExecuteQueryAsync(new GetDebitWalletQuery(_walletId));

        Assert.That(bankConnectionProcess.Status, Is.EqualTo(BankConnectionProcessStatus.Completed.Value));
        Assert.That(wallet.BankConnectionId, Is.EqualTo(bankConnectionProcessId));
        Assert.That(wallet.BankAccountId, Is.EqualTo(bankAccountId));
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

    private async Task CreateBankConnectionWithMultipleBankAccounts(Guid bankConnectionProcessId)
    {
        SaltEdgeHttpClientMocker.MockFetchConnectionSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockFetchConsentSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockFetchAccountsSuccessfulResponse(hasMultipleAccounts: true);

        await WalletsModule.ExecuteCommandAsync(new CreateBankConnectionCommand(
            bankConnectionProcessId,
            BankConnectionProcessingData.ExternalConnectionId));
    }

    private async Task<Guid> GetBankAccountByExternalId(string externalId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = @"SELECT id FROM wallets.bank_accounts a WHERE a.external_id = @ExternalId";

        return await connection.QuerySingleAsync<Guid>(sql, new { ExternalId = externalId });
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
