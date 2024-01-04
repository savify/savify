using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.EditCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.CashWallets;

[TestFixture]
public class EditCashWalletTests : TestBase
{
    [Test]
    public async Task EditCashWalletCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId,
            "Cash wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        await FinanceTrackingModule.ExecuteCommandAsync(new EditCashWalletCommand(
            userId,
            walletId,
            "New title",
            "USD",
            2000,
            "#000000",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false));

        var editedWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(walletId, userId));

        Assert.That(editedWallet!.UserId, Is.EqualTo(userId));
        Assert.That(editedWallet.Title, Is.EqualTo("New title"));
        Assert.That(editedWallet.Balance, Is.EqualTo(2000));
        Assert.That(editedWallet.Currency, Is.EqualTo("USD"));

        Assert.That(editedWallet.ViewMetadata.Color, Is.EqualTo("#000000"));
        Assert.That(editedWallet.ViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/new-wallet.png"));
        Assert.That(editedWallet.ViewMetadata.IsConsideredInTotalBalance, Is.False);
    }

    [Test]
    public void EditCashWalletCommand_WhenWalletDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var userId = Guid.NewGuid();
        var walletId = Guid.NewGuid();

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new EditCashWalletCommand(
            userId,
            walletId,
            "New title",
            "USD",
            2000,
            "#000000",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false)), Throws.TypeOf<NotFoundRepositoryException<CashWallet>>());
    }

    [Test]
    public async Task EditCashWalletCommand_WhenWalletDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId,
            "Cash wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new EditCashWalletCommand(
            Guid.NewGuid(),
            walletId,
            "New title",
            "USD",
            2000,
            "#000000",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false)), Throws.TypeOf<AccessDeniedException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    public void EditNewCashWalletCommand_WhenTitleIsInvalid_ThrowsInvalidCommandException(string title)
    {
        var command = new EditCashWalletCommand(
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
    public void EditNewCashWalletCommand_WhenCurrencyIsInvalid_ThrowsInvalidCommandException(string currency)
    {
        var command = new EditCashWalletCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Cash wallet",
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
    public void EditNewCashWalletCommand_WhenColorIsInvalid_ThrowsInvalidCommandException(string color)
    {
        var command = new EditCashWalletCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Cash wallet",
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
    public void EditNewCashWalletCommand_WhenIconUrlIsInvalid_ThrowsInvalidCommandException(string iconUrl)
    {
        var command = new EditCashWalletCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Cash wallet",
            "PLN",
            1000,
            "#ffffff",
            iconUrl,
            true);

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }
}
