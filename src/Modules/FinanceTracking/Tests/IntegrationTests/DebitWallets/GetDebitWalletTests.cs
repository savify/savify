using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.DebitWallets;

[TestFixture]
public class GetDebitWalletTests : TestBase
{
    [Test]
    public async Task GetDebitWalletQuery_WhenWalletDoesNotExist_ReturnsNull()
    {
        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(Guid.NewGuid(), Guid.NewGuid()));

        Assert.That(transfer, Is.Null);
    }

    [Test]
    public async Task GetDebitWalletQuery_WhenWalletDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(new AddNewDebitWalletCommand(
            userId,
            "Debit wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteQueryAsync(
                new GetDebitWalletQuery(walletId, Guid.NewGuid())),
            Throws.TypeOf<AccessDeniedException>());
    }
}
