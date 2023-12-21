using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.EditDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.DebitWallets;

[TestFixture]
public class EditDebitWalletTests : TestBase
{
    [Test]
    public async Task EditDebitWalletCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(new AddNewDebitWalletCommand(
            userId,
            "Debit wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        await FinanceTrackingModule.ExecuteCommandAsync(new EditDebitWalletCommand(
            userId,
            walletId,
            "New title",
            "USD",
            2000,
            "#000000",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false));

        var editedWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(walletId));

        Assert.That(editedWallet!.UserId, Is.EqualTo(userId));
        Assert.That(editedWallet.Title, Is.EqualTo("New title"));
        Assert.That(editedWallet.Balance, Is.EqualTo(2000));
        Assert.That(editedWallet.Currency, Is.EqualTo("USD"));

        Assert.That(editedWallet.ViewMetadata.Color, Is.EqualTo("#000000"));
        Assert.That(editedWallet.ViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/new-wallet.png"));
        Assert.That(editedWallet.ViewMetadata.IsConsideredInTotalBalance, Is.False);
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    public void EditNewDebitWalletCommand_WhenTitleIsInvalid_ThrowsInvalidCommandException(string title)
    {
        var command = new EditDebitWalletCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            title,
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("pl")]
    [TestCase("invalid")]
    public void EditNewDebitWalletCommand_WhenCurrencyIsInvalid_ThrowsInvalidCommandException(string currency)
    {
        var command = new EditDebitWalletCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Debit wallet",
            currency,
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("invalid")]
    [TestCase("#FFFFFFF")]
    public void EditNewDebitWalletCommand_WhenColorIsInvalid_ThrowsInvalidCommandException(string color)
    {
        var command = new EditDebitWalletCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Debit wallet",
            "PLN",
            1000,
            color,
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("invalid")]
    public void EditNewDebitWalletCommand_WhenIconUrlIsInvalid_ThrowsInvalidCommandException(string iconUrl)
    {
        var command = new EditDebitWalletCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Debit wallet",
            "PLN",
            1000,
            "#ffffff",
            iconUrl,
            true);

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }
}
