using App.Modules.FinanceTracking.Application.BankConnectionProcessing.ChooseBankAccountToConnect;
using App.Modules.FinanceTracking.Application.BankConnectionProcessing.CreateBankConnection;
using App.Modules.FinanceTracking.Application.BankConnectionProcessing.GetBankConnectionProcess;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.IntegrationTests.SeedData;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.BankConnectionProcessing;

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

        await FinanceTrackingModule.ExecuteCommandAsync(new ChooseBankAccountToConnectCommand(
            bankConnectionProcessId,
            BankConnectionProcessingData.UserId,
            bankAccountId
        ));

        var bankConnectionProcess = await FinanceTrackingModule.ExecuteQueryAsync(new GetBankConnectionProcessQuery(bankConnectionProcessId));
        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(_walletId));

        Assert.That(bankConnectionProcess.Status, Is.EqualTo(BankConnectionProcessStatus.State.Completed.ToString()));
        Assert.That(wallet.BankConnectionId, Is.EqualTo(bankConnectionProcessId));
        Assert.That(wallet.BankAccountId, Is.EqualTo(bankAccountId));
    }

    private async Task<Guid> InitiateBankConnectionProcess()
    {
        SaltEdgeHttpClientMocker.MockCreateCustomerSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockCreateConnectSessionSuccessfulResponse();

        _walletId = await AddDebitWalletFor(BankConnectionProcessingData.UserId);
        var result = await FinanceTrackingModule.ExecuteCommandAsync(new ConnectBankAccountToDebitWalletCommand(
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

        await FinanceTrackingModule.ExecuteCommandAsync(new CreateBankConnectionCommand(
            bankConnectionProcessId,
            BankConnectionProcessingData.ExternalConnectionId));
    }

    private async Task<Guid> GetBankAccountByExternalId(string externalId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT id FROM {DatabaseConfiguration.Schema.Name}.bank_accounts a WHERE a.external_id = @ExternalId";

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

        return await FinanceTrackingModule.ExecuteCommandAsync(command);
    }
}
