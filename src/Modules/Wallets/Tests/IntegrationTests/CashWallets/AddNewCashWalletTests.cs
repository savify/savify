using App.Modules.Wallets.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.Wallets.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.Wallets.IntegrationTests.SeedWork;

namespace App.Modules.Wallets.IntegrationTests.CashWallets;

[TestFixture]
public class AddNewCashWalletTests : TestBase
{
    [Test]
    public async Task AddNewCashWalletCommand_Tests()
    {
        var command = new AddNewCashWalletCommand(
            Guid.NewGuid(),
            "Cash wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        var walletId = await WalletsModule.ExecuteCommandAsync(command);

        var wallet = await WalletsModule.ExecuteQueryAsync(new GetCashWalletQuery(walletId));

        Assert.IsNotNull(wallet);
        Assert.That(wallet.UserId, Is.EqualTo(command.UserId));
        Assert.That(wallet.Title, Is.EqualTo(command.Title));
        Assert.That(wallet.Balance, Is.EqualTo(command.Balance));

        Assert.IsNotNull(wallet.ViewMetadata);
        Assert.That(wallet.ViewMetadata.WalletId, Is.EqualTo(walletId));
        Assert.That(wallet.ViewMetadata.Color, Is.EqualTo("#ffffff"));
        Assert.That(wallet.ViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/wallet.png"));
        Assert.That(wallet.ViewMetadata.IsConsideredInTotalBalance, Is.True);
    }
}
