using App.Modules.Wallets.Application.BankConnectionProcessing.CreateBankConnection;
using App.Modules.Wallets.Application.BankConnectionProcessing.GetBankConnectionProcess;
using App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.Wallets.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.Wallets.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.IntegrationTests.SeedData;
using App.Modules.Wallets.IntegrationTests.SeedWork;

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
        Assert.That(bankConnectionProcess.Status, Is.EqualTo(BankConnectionProcessStatus.Completed.Value));
        Assert.That(wallet.Balance, Is.EqualTo((int)BankConnectionProcessingData.ExternalUSDAccountBalance * 100));
        Assert.That(wallet.Currency, Is.EqualTo(BankConnectionProcessingData.ExternalUSDAccountCurrency));
        Assert.That(wallet.BankConnectionId, Is.EqualTo(bankConnectionProcess.Id));
        Assert.That(wallet.BankAccountId, Is.Not.Null);
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

        return result.Id;
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
