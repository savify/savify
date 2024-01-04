using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Wallets.DebitWallets;

[TestFixture]
public class AddNewDebitWalletTests : TestBase
{
    [Test]
    public async Task AddNewDebitWalletCommand_Tests()
    {
        var command = new AddNewDebitWalletCommand(
            Guid.NewGuid(),
            "Debit wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(walletId, command.UserId));

        Assert.That(wallet, Is.Not.Null);
        Assert.That(wallet!.Id, Is.EqualTo(walletId));
        Assert.That(wallet.UserId, Is.EqualTo(command.UserId));
        Assert.That(wallet.Title, Is.EqualTo(command.Title));
        Assert.That(wallet.Currency, Is.EqualTo(command.Currency));
        Assert.That(wallet.Balance, Is.EqualTo(command.Balance));

        Assert.That(wallet.ViewMetadata, Is.Not.Null);
        Assert.That(wallet.ViewMetadata.WalletId, Is.EqualTo(walletId));
        Assert.That(wallet.ViewMetadata.Color, Is.EqualTo("#ffffff"));
        Assert.That(wallet.ViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/wallet.png"));
        Assert.That(wallet.ViewMetadata.IsConsideredInTotalBalance, Is.True);
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    public void AddNewDebitWalletCommand_WhenTitleIsInvalid_ThrowsInvalidCommandException(string title)
    {
        var command = new AddNewDebitWalletCommand(
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
    public void AddNewDebitWalletCommand_WhenCurrencyIsInvalid_ThrowsInvalidCommandException(string currency)
    {
        var command = new AddNewDebitWalletCommand(
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
    public void AddNewDebitWalletCommand_WhenColorIsInvalid_ThrowsInvalidCommandException(string color)
    {
        var command = new AddNewDebitWalletCommand(
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
    public void AddNewDebitWalletCommand_WhenIconUrlIsInvalid_ThrowsInvalidCommandException(string iconUrl)
    {
        var command = new AddNewDebitWalletCommand(
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
