using App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.Wallets.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.Wallets.IntegrationTests.SeedWork;

namespace App.Modules.Wallets.IntegrationTests.DebitWallets;

[TestFixture]
public class AddNewDebitWalletTests : TestBase
{
    [Test]
    public async Task AddNewDebitWalletCommand_Tests()
    {
        var command = new AddNewDebitWalletCommand(
            userId: Guid.NewGuid(),
            title: "Debit wallet",
            currency: "PLN",
            balance: 1000);
        var walletId = await WalletsModule.ExecuteCommandAsync(command);

        var addedDebitWallet = await WalletsModule.ExecuteQueryAsync(new GetDebitWalletQuery(walletId));

        Assert.IsNotNull(addedDebitWallet);
        Assert.That(addedDebitWallet.Id, Is.EqualTo(walletId));
        Assert.That(addedDebitWallet.UserId, Is.EqualTo(command.UserId));
        Assert.That(addedDebitWallet.Title, Is.EqualTo(command.Title));
        Assert.That(addedDebitWallet.Currency, Is.EqualTo(command.Currency));
        Assert.That(addedDebitWallet.Balance, Is.EqualTo(command.Balance));

        Assert.IsNotNull(addedDebitWallet.ViewMetadata);
        Assert.That(addedDebitWallet.ViewMetadata.WalletId, Is.EqualTo(walletId));
    }
}
