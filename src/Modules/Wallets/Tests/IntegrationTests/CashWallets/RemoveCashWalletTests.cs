using App.Modules.Wallets.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.Wallets.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.Wallets.Application.Wallets.CashWallets.RemoveCashWallet;
using App.Modules.Wallets.IntegrationTests.SeedWork;

namespace App.Modules.Wallets.IntegrationTests.CashWallets;

[TestFixture]
public class RemoveCashWalletTests : TestBase
{
    [Test]
    public async Task RemoveCashWalletCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var walletId = await WalletsModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId,
            "Cash wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        await WalletsModule.ExecuteCommandAsync(new RemoveCashWalletCommand(userId, walletId));

        var removedWallet = await WalletsModule.ExecuteQueryAsync(new GetCashWalletQuery(walletId));

        Assert.That(removedWallet.Id, Is.EqualTo(walletId));
        Assert.That(removedWallet.UserId, Is.EqualTo(userId));
        Assert.That(removedWallet.IsRemoved, Is.True);
    }
}
