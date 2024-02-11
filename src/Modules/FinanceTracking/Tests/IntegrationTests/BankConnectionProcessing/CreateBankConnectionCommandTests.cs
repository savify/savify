using App.Modules.FinanceTracking.Application.BankConnectionProcessing.CreateBankConnection;
using App.Modules.FinanceTracking.Application.BankConnectionProcessing.GetBankConnectionProcess;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Users.FinanceTrackingSettings.CreateUserFinanceTrackingSettings;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.BankConnectionProcessing;

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

        await FinanceTrackingModule.ExecuteCommandAsync(new CreateBankConnectionCommand(bankConnectionProcessId, BankConnectionProcessingData.ExternalConnectionId));

        var bankConnectionProcess = await FinanceTrackingModule.ExecuteQueryAsync(new GetBankConnectionProcessQuery(bankConnectionProcessId, BankConnectionProcessingData.UserId));
        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(_walletId, bankConnectionProcess!.UserId));

        Assert.That(bankConnectionProcess, Is.Not.Null);
        Assert.That(bankConnectionProcess.Status, Is.EqualTo(BankConnectionProcessStatus.State.Completed.ToString()));
        Assert.That(await CountBankAccountsInConnection(bankConnectionProcess.Id), Is.EqualTo(1));
        Assert.That(wallet!.Balance, Is.EqualTo((int)BankConnectionProcessingData.ExternalUsdAccountBalance * 100));
        Assert.That(wallet.Currency, Is.EqualTo(BankConnectionProcessingData.ExternalUsdAccountCurrency));
        Assert.That(wallet.BankConnectionId, Is.EqualTo(bankConnectionProcess.Id));
        Assert.That(wallet.BankAccountId, Is.Not.Null);
    }

    [Test]
    public async Task CreateBankConnectionCommand_WithMultipleAccounts_SetWaitingForAccountChoosingStatus_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchConnectionSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockFetchConsentSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockFetchAccountsSuccessfulResponse(hasMultipleAccounts: true);

        var bankConnectionProcessId = await InitiateBankConnectionProcess();

        await FinanceTrackingModule.ExecuteCommandAsync(new CreateBankConnectionCommand(bankConnectionProcessId, BankConnectionProcessingData.ExternalConnectionId));

        var bankConnectionProcess = await FinanceTrackingModule.ExecuteQueryAsync(new GetBankConnectionProcessQuery(bankConnectionProcessId, BankConnectionProcessingData.UserId));

        Assert.That(bankConnectionProcess, Is.Not.Null);
        Assert.That(bankConnectionProcess!.Status, Is.EqualTo(BankConnectionProcessStatus.State.WaitingForAccountChoosing.ToString()));
        Assert.That(await CountBankAccountsInConnection(bankConnectionProcess.Id), Is.EqualTo(2));
    }

    private async Task<Guid> InitiateBankConnectionProcess()
    {
        SaltEdgeHttpClientMocker.MockCreateCustomerSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockCreateConnectSessionSuccessfulResponse();

        await AddUserSettingsFor(BankConnectionProcessingData.UserId);

        _walletId = await AddDebitWalletFor(BankConnectionProcessingData.UserId);
        var result = await FinanceTrackingModule.ExecuteCommandAsync(new ConnectBankAccountToDebitWalletCommand(
            BankConnectionProcessingData.UserId,
            _walletId,
            BankConnectionProcessingData.BankId));

        return result.Success.Id;
    }

    private async Task<int> CountBankAccountsInConnection(Guid bankConnectionId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT COUNT(*) FROM {DatabaseConfiguration.Schema.Name}.bank_accounts a WHERE a.bank_connection_id = @ConnectionId";

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

        return await FinanceTrackingModule.ExecuteCommandAsync(command);
    }

    private async Task AddUserSettingsFor(Guid userId)
    {
        var command = new CreateUserFinanceTrackingSettingsCommand(
            id: Guid.NewGuid(),
            correlationId: Guid.NewGuid(),
            userId,
            countryCode: "US",
            preferredLanguage: BankConnectionProcessingData.UserLanguage);

        await FinanceTrackingModule.ExecuteCommandAsync(command);
    }
}
