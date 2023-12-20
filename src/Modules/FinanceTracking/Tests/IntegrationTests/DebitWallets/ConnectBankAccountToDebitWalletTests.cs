using App.Modules.FinanceTracking.Application.BankConnectionProcessing.GetBankConnectionProcess;
using App.Modules.FinanceTracking.Application.Configuration.Data;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.IntegrationTests.BankConnectionProcessing;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.FinanceTracking.IntegrationTests.DebitWallets;

[TestFixture]
public class ConnectBankAccountToDebitWalletTests : TestBase
{
    [Test]
    public async Task ConnectBankAccountToDebitWalletCommand_Test()
    {
        SaltEdgeHttpClientMocker.MockCreateCustomerSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockCreateConnectSessionSuccessfulResponse();

        var walletId = await AddDebitWalletFor(BankConnectionProcessingData.UserId);

        var result = await FinanceTrackingModule.ExecuteCommandAsync(new ConnectBankAccountToDebitWalletCommand(BankConnectionProcessingData.UserId, walletId, BankConnectionProcessingData.BankId));
        var bankConnectionProcess = await FinanceTrackingModule.ExecuteQueryAsync(new GetBankConnectionProcessQuery(result.Success.Id));
        var saltEdgeCustomer = await GetSaltEdgeCustomerByUserId(BankConnectionProcessingData.UserId);

        Assert.That(result.Success.RedirectUrl, Is.EqualTo(BankConnectionProcessingData.ExpectedRedirectUrl));

        Assert.That(bankConnectionProcess, Is.Not.Null);
        Assert.That(bankConnectionProcess!.UserId, Is.EqualTo(BankConnectionProcessingData.UserId));
        Assert.That(bankConnectionProcess.BankId, Is.EqualTo(BankConnectionProcessingData.BankId));
        Assert.That(bankConnectionProcess.WalletId, Is.EqualTo(walletId));
        Assert.That(bankConnectionProcess.WalletType, Is.EqualTo(WalletType.Debit.Value));
        Assert.That(bankConnectionProcess.Status, Is.EqualTo(BankConnectionProcessStatus.State.Redirected.ToString()));

        Assert.That(saltEdgeCustomer, Is.Not.Null);
        Assert.That(saltEdgeCustomer!.Id, Is.EqualTo(BankConnectionProcessingData.ExternalCustomerId));
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

    private async Task<SaltEdgeCustomerDto?> GetSaltEdgeCustomerByUserId(Guid userId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT id, identifier FROM {DatabaseConfiguration.Schema.Name}.salt_edge_customers WHERE identifier = @UserId";

        return await connection.QuerySingleOrDefaultAsync<SaltEdgeCustomerDto>(sql, new { UserId = userId });
    }

    private class SaltEdgeCustomerDto
    {
        public required string Id { get; init; }

        public Guid Identifier { get; init; }
    }
}
