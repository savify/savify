using App.BuildingBlocks.Application.Exceptions;
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

        var wallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(walletId, command.UserId));

        Assert.That(wallet, Is.Not.Null);
        Assert.That(wallet!.Id, Is.EqualTo(walletId));
        Assert.That(wallet.UserId, Is.EqualTo(command.UserId));
        Assert.That(wallet.Title, Is.EqualTo(command.Title));
        Assert.That(wallet.AvailableBalance, Is.EqualTo(command.AvailableBalance));
        Assert.That(wallet.CreditLimit, Is.EqualTo(command.CreditLimit));
        Assert.That(wallet.Currency, Is.EqualTo(command.Currency));

        Assert.That(wallet.ViewMetadata, Is.Not.Null);
        Assert.That(wallet.ViewMetadata.WalletId, Is.EqualTo(walletId));
        Assert.That(wallet.ViewMetadata.Color, Is.EqualTo("#ffffff"));
        Assert.That(wallet.ViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/wallet.png"));
        Assert.That(wallet.ViewMetadata.IsConsideredInTotalBalance, Is.True);
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    public void AddNewCreditWalletCommand_WhenTitleIsInvalid_ThrowsInvalidCommandException(string title)
    {
        var command = new AddNewCreditWalletCommand(
            Guid.NewGuid(),
            title,
            "PLN",
            500,
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public void AddNewCreditWalletCommand_WhenCreditLimitIsNegative_ThrowsInvalidCommandException()
    {
        var command = new AddNewCreditWalletCommand(
            Guid.NewGuid(),
            "Credit wallet",
            "PLN",
            500,
            -100,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public void AddNewCreditWalletCommand_WhenAvailableBalanceIsHigherThatCreditLimit_ThrowsInvalidCommandException()
    {
        var command = new AddNewCreditWalletCommand(
            Guid.NewGuid(),
            "Credit wallet",
            "PLN",
            1500,
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
    public void AddNewCreditWalletCommand_WhenCurrencyIsInvalid_ThrowsInvalidCommandException(string currency)
    {
        var command = new AddNewCreditWalletCommand(
            Guid.NewGuid(),
            "Credit wallet",
            currency,
            500,
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
    public void AddNewCreditWalletCommand_WhenColorIsInvalid_ThrowsInvalidCommandException(string color)
    {
        var command = new AddNewCreditWalletCommand(
            Guid.NewGuid(),
            "Credit wallet",
            "PLN",
            500,
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
    public void AddNewCreditWalletCommand_WhenIconUrlIsInvalid_ThrowsInvalidCommandException(string iconUrl)
    {
        var command = new AddNewCreditWalletCommand(
            Guid.NewGuid(),
            "Credit wallet",
            "PLN",
            500,
            1000,
            "#ffffff",
            iconUrl,
            true);

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }
}
