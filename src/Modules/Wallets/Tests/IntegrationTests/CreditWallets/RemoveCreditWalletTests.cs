using App.Modules.Wallets.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.Wallets.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.Wallets.Application.Wallets.CreditWallets.RemoveCreditWallet;
using App.Modules.Wallets.IntegrationTests.SeedWork;

namespace App.Modules.Wallets.IntegrationTests.CreditWallets;

[TestFixture]
public class RemoveCreditWalletTests : TestBase
{
    [Test]
    public async Task RemoveCreditWalletCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var walletId = await WalletsModule.ExecuteCommandAsync(new AddNewCreditWalletCommand(
            userId,
            "Credit wallet",
            "PLN",
            1000,
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        await WalletsModule.ExecuteCommandAsync(new RemoveCreditWalletCommand(userId, walletId));

        var removedWallet = await WalletsModule.ExecuteQueryAsync(new GetCreditWalletQuery(walletId));

        Assert.That(removedWallet.Id, Is.EqualTo(walletId));
        Assert.That(removedWallet.UserId, Is.EqualTo(userId));
        Assert.That(removedWallet.IsRemoved, Is.True);
    }
}
