using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.EditCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.CreditWallets;

[TestFixture]
public class EditCreditWalletTests : TestBase
{
    [Test]
    public async Task EditCreditWalletCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCreditWalletCommand(
            userId,
            "Debit wallet",
            "PLN",
            1000,
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        await FinanceTrackingModule.ExecuteCommandAsync(new EditCreditWalletCommand(
            userId,
            walletId,
            "New title",
            "USD",
            2000,
            2000,
            "#000000",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false));

        var editedWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(walletId));

        Assert.That(editedWallet.UserId, Is.EqualTo(userId));
        Assert.That(editedWallet.Title, Is.EqualTo("New title"));
        Assert.That(editedWallet.AvailableBalance, Is.EqualTo(2000));
        Assert.That(editedWallet.CreditLimit, Is.EqualTo(2000));
        Assert.That(editedWallet.Currency, Is.EqualTo("USD"));

        Assert.That(editedWallet.ViewMetadata.Color, Is.EqualTo("#000000"));
        Assert.That(editedWallet.ViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/new-wallet.png"));
        Assert.That(editedWallet.ViewMetadata.IsConsideredInTotalBalance, Is.False);
    }
}
