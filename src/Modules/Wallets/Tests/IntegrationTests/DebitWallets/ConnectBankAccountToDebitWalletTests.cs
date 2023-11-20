using App.Modules.Wallets.Application.BankConnectionProcessing.GetBankConnectionProcess;
using App.Modules.Wallets.Application.Configuration.Data;
using App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.Wallets.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.IntegrationTests.BankConnectionProcessing;
using App.Modules.Wallets.IntegrationTests.SeedData;
using App.Modules.Wallets.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.Wallets.IntegrationTests.DebitWallets;

[TestFixture]
public class ConnectBankAccountToDebitWalletTests : TestBase
{
    [Test]
    public async Task ConnectBankAccountToDebitWalletCommand_Test()
    {
        SaltEdgeHttpClientMocker.MockCreateCustomerSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockCreateConnectSessionSuccessfulResponse();

        var walletId = await AddDebitWalletFor(BankConnectionProcessingData.UserId);

        var result = await WalletsModule.ExecuteCommandAsync(new ConnectBankAccountToDebitWalletCommand(BankConnectionProcessingData.UserId, walletId, BankConnectionProcessingData.BankId));
        var bankConnectionProcess = await WalletsModule.ExecuteQueryAsync(new GetBankConnectionProcessQuery(result.Success.Id));
        var saltEdgeCustomer = await GetSaltEdgeCustomerByUserId(BankConnectionProcessingData.UserId);

        Assert.That(result.Success.RedirectUrl, Is.EqualTo(BankConnectionProcessingData.ExpectedRedirectUrl));

        Assert.That(bankConnectionProcess, Is.Not.Null);
        Assert.That(bankConnectionProcess.UserId, Is.EqualTo(BankConnectionProcessingData.UserId));
        Assert.That(bankConnectionProcess.BankId, Is.EqualTo(BankConnectionProcessingData.BankId));
        Assert.That(bankConnectionProcess.WalletId, Is.EqualTo(walletId));
        Assert.That(bankConnectionProcess.WalletType, Is.EqualTo(WalletType.Debit.Value));
        Assert.That(bankConnectionProcess.Status, Is.EqualTo(BankConnectionProcessStatus.State.Redirected.ToString()));

        Assert.That(saltEdgeCustomer, Is.Not.Null);
        Assert.That(saltEdgeCustomer.Id, Is.EqualTo(BankConnectionProcessingData.ExternalCustomerId));
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

    private async Task<SaltEdgeCustomerDto?> GetSaltEdgeCustomerByUserId(Guid userId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT id, identifier FROM {DatabaseConfiguration.Schema.Name}.salt_edge_customers WHERE identifier = @UserId";

        return await connection.QuerySingleOrDefaultAsync<SaltEdgeCustomerDto>(sql, new { UserId = userId });
    }

    private class SaltEdgeCustomerDto
    {
        public string Id { get; set; }

        public Guid Identifier { get; set; }
    }
}
