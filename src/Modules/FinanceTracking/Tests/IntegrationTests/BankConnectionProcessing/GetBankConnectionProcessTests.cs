using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.BankConnectionProcessing.GetBankConnectionProcess;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.BankConnectionProcessing;

[TestFixture]
public class GetBankConnectionProcessTests : TestBase
{
    [Test]
    public async Task GetBankConnectionProcessQuery_WhenBankConnectionProcessDoesNotExist_ReturnsNull()
    {
        var bankConnectionProcess = await FinanceTrackingModule.ExecuteQueryAsync(
            new GetBankConnectionProcessQuery(Guid.NewGuid(), BankConnectionProcessingData.UserId));

        Assert.That(bankConnectionProcess, Is.Null);
    }

    [Test]
    public async Task GetBankConnectionProcessQuery_WhenBankConnectionProcessDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        SaltEdgeHttpClientMocker.MockCreateCustomerSuccessfulResponse();
        SaltEdgeHttpClientMocker.MockCreateConnectSessionSuccessfulResponse();
        var walletId = await AddDebitWalletFor(BankConnectionProcessingData.UserId);
        var result = await FinanceTrackingModule.ExecuteCommandAsync(new ConnectBankAccountToDebitWalletCommand(BankConnectionProcessingData.UserId, walletId, BankConnectionProcessingData.BankId));

        Assert.That(() => FinanceTrackingModule.ExecuteQueryAsync(
            new GetBankConnectionProcessQuery(result.Success.Id, Guid.NewGuid())), Throws.TypeOf<AccessDeniedException>());
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
