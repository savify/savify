using App.Modules.Wallets.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.Wallets.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.Wallets.IntegrationTests.SeedWork;

namespace App.Modules.Wallets.IntegrationTests.CreditWallets;

[TestFixture]
public class AddNewCreditWalletTests : TestBase
{
    [Test]
    public async Task AddNewCreditWalletCommand_AddsNewCreditWallet()
    {
        var command = new AddNewCreditWalletCommand(
            userId: Guid.NewGuid(),
            title: "Credit wallet",
            currency: "PLN",
            availableBalance: 500,
            creditLimit: 1000);
        var walletId = await WalletsModule.ExecuteCommandAsync(command);

        var addedCreditWallet = await WalletsModule.ExecuteQueryAsync(new GetCreditWalletQuery(walletId));

        Assert.IsNotNull(addedCreditWallet);
        Assert.That(addedCreditWallet.Id, Is.EqualTo(walletId));
        Assert.That(addedCreditWallet.Title, Is.EqualTo(command.Title));
        Assert.That(addedCreditWallet.AvailableBalance, Is.EqualTo(command.AvailableBalance));
        Assert.That(addedCreditWallet.CreditLimit, Is.EqualTo(command.CreditLimit));
        Assert.That(addedCreditWallet.Currency, Is.EqualTo(command.Currency));

        Assert.IsNotNull(addedCreditWallet.ViewMetadata);
        Assert.That(addedCreditWallet.ViewMetadata.WalletId, Is.EqualTo(walletId));
    }
}
