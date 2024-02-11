using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.BankConnectionProcessing.ChooseBankAccountToConnect;
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
public class ChooseBankAccountToConnectCommandTests : TestBase
{
    private Guid _walletId;

    [Test]
    public async Task ChooseBankAccountToConnectCommand_Test()
    {
        var bankConnectionProcessId = await InitiateBankConnectionProcess();
        await CreateBankConnectionWithMultipleBankAccounts(bankConnectionProcessId);

        var bankAccountId = await GetBankAccountByExternalId(BankConnectionProcessingData.ExternalUsdAccountId);

        await FinanceTrackingModule.ExecuteCommandAsync(new ChooseBankAccountToConnectCommand(
            bankConnectionProcessId,
            BankConnectionProcessingData.UserId,
            bankAccountId
        ));

        var bankConnectionProcess = await FinanceTrackingModule.ExecuteQueryAsync(new GetBankConnectionProcessQuery(bankConnectionProcessId, BankConnectionProcessingData.UserId));
        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(_walletId, BankConnectionProcessingData.UserId));

        Assert.That(bankConnectionProcess!.Status, Is.EqualTo(BankConnectionProcessStatus.State.Completed.ToString()));
        Assert.That(wallet!.BankConnectionId, Is.EqualTo(bankConnectionProcessId));
        Assert.That(wallet.BankAccountId, Is.EqualTo(bankAccountId));
    }

    [Test]
    public void ChooseBankAccountToConnectCommand_WhenBankConnectionProcessDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new ChooseBankAccountToConnectCommand(
            Guid.NewGuid(),
            BankConnectionProcessingData.UserId,
            Guid.NewGuid())), Throws.TypeOf<NotFoundRepositoryException<BankConnectionProcess>>());
    }

    [Test]
    public async Task ChooseBankAccountToConnectCommand_WhenBankConnectionProcessDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var bankConnectionProcessId = await InitiateBankConnectionProcess();
        await CreateBankConnectionWithMultipleBankAccounts(bankConnectionProcessId);

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new ChooseBankAccountToConnectCommand(
            bankConnectionProcessId,
            Guid.NewGuid(),
            Guid.NewGuid())), Throws.TypeOf<AccessDeniedException>());
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
