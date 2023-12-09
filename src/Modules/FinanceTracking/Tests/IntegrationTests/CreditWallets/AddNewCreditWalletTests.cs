using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.CreditWallets;

[TestFixture]
public class AddNewCreditWalletTests : TestBase
{
    [Test]
    public async Task AddNewCreditWalletCommand_AddsNewCreditWallet()
    {
        var command = new AddNewCreditWalletCommand(
            Guid.NewGuid(),
            "Credit wallet",
            "PLN",
            500,
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(walletId));

        Assert.IsNotNull(wallet);
        Assert.That(wallet.Id, Is.EqualTo(walletId));
        Assert.That(wallet.UserId, Is.EqualTo(command.UserId));
        Assert.That(wallet.Title, Is.EqualTo(command.Title));
        Assert.That(wallet.AvailableBalance, Is.EqualTo(command.AvailableBalance));
        Assert.That(wallet.CreditLimit, Is.EqualTo(command.CreditLimit));
        Assert.That(wallet.Currency, Is.EqualTo(command.Currency));

        Assert.IsNotNull(wallet.ViewMetadata);
        Assert.That(wallet.ViewMetadata.WalletId, Is.EqualTo(walletId));
        Assert.That(wallet.ViewMetadata.Color, Is.EqualTo("#ffffff"));
        Assert.That(wallet.ViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/wallet.png"));
        Assert.That(wallet.ViewMetadata.IsConsideredInTotalBalance, Is.True);
    }
}
