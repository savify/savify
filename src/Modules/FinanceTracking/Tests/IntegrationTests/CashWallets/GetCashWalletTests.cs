using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.CashWallets;

[TestFixture]
public class GetCashWalletTests : TestBase
{
    [Test]
    public async Task GetCashWalletQuery_WhenWalletDoesNotExist_ReturnsNull()
    {
        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(Guid.NewGuid(), Guid.NewGuid()));

        Assert.That(transfer, Is.Null);
    }

    [Test]
    public async Task GetCashWalletQuery_WhenWalletDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId,
            "Cash wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteQueryAsync(
                new GetCashWalletQuery(walletId, Guid.NewGuid())),
            Throws.TypeOf<AccessDeniedException>());
    }
}
