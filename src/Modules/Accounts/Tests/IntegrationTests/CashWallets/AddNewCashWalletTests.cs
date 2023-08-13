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
            1000);
        var walletId = await WalletsModule.ExecuteCommandAsync(command);

        var addedCashWallet = await WalletsModule.ExecuteQueryAsync(new GetCashWalletQuery(walletId));

        Assert.IsNotNull(addedCashWallet);
        Assert.That(addedCashWallet.UserId, Is.EqualTo(command.UserId));
        Assert.That(addedCashWallet.Title, Is.EqualTo(command.Title));
        Assert.That(addedCashWallet.Balance, Is.EqualTo(command.Balance));

        Assert.IsNotNull(addedCashWallet.ViewMetadata);
        Assert.That(addedCashWallet.ViewMetadata.WalletId, Is.EqualTo(walletId));
    }
}