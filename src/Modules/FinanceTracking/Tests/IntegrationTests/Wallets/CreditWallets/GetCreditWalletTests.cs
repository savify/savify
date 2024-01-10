using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Wallets.CreditWallets;

[TestFixture]
public class GetCreditWalletTests : TestBase
{
    [Test]
    public async Task GetCreditWalletQuery_WhenWalletDoesNotExist_ReturnsNull()
    {
        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(Guid.NewGuid(), Guid.NewGuid()));

        Assert.That(transfer, Is.Null);
    }

    [Test]
    public async Task GetCreditWalletQuery_WhenWalletDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCreditWalletCommand(
            userId,
            "Credit wallet",
            "PLN",
            1000,
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteQueryAsync(
                new GetCreditWalletQuery(walletId, Guid.NewGuid())),
            Throws.TypeOf<AccessDeniedException>());
    }
}
