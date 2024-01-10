using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.EditCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Wallets.CreditWallets;

[TestFixture]
public class EditCreditWalletTests : TestBase
{
    [Test]
    public async Task EditCreditWalletCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCreditWalletCommand(
            userId,
            "Credit wallet",
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

        var editedWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(walletId, userId));

        Assert.That(editedWallet!.UserId, Is.EqualTo(userId));
        Assert.That(editedWallet.Title, Is.EqualTo("New title"));
        Assert.That(editedWallet.AvailableBalance, Is.EqualTo(2000));
        Assert.That(editedWallet.CreditLimit, Is.EqualTo(2000));
        Assert.That(editedWallet.Currency, Is.EqualTo("USD"));

        Assert.That(editedWallet.ViewMetadata.Color, Is.EqualTo("#000000"));
        Assert.That(editedWallet.ViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/new-wallet.png"));
        Assert.That(editedWallet.ViewMetadata.IsConsideredInTotalBalance, Is.False);
    }

    [Test]
    public void EditCreditWalletCommand_WhenWalletDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var userId = Guid.NewGuid();
        var walletId = Guid.NewGuid();

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new EditCreditWalletCommand(
            userId,
            walletId,
            "New title",
            "USD",
            2000,
            2000,
            "#000000",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false)), Throws.TypeOf<NotFoundRepositoryException<CreditWallet>>());
    }

    [Test]
    public async Task EditCreditWalletCommand_WhenWalletDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var walletId = await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCreditWalletCommand(
            userId,
            "Credit wallet",
            "PLN",
            1000,
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true));

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new EditCreditWalletCommand(
            Guid.NewGuid(),
            walletId,
            "New title",
            "USD",
            2000,
            2000,
            "#000000",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false)), Throws.TypeOf<AccessDeniedException>());
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    public void EditNewCreditWalletCommand_WhenTitleIsInvalid_ThrowsInvalidCommandException(string title)
    {
        var command = new EditCreditWalletCommand(
            Guid.NewGuid(),
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
        var command = new EditCreditWalletCommand(
            Guid.NewGuid(),
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
        var command = new EditCreditWalletCommand(
            Guid.NewGuid(),
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
    public void EditNewCreditWalletCommand_WhenCurrencyIsInvalid_ThrowsInvalidCommandException(string currency)
    {
        var command = new EditCreditWalletCommand(
            Guid.NewGuid(),
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
    public void EditNewCreditWalletCommand_WhenColorIsInvalid_ThrowsInvalidCommandException(string color)
    {
        var command = new EditCreditWalletCommand(
            Guid.NewGuid(),
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
    public void EditNewCreditWalletCommand_WhenIconUrlIsInvalid_ThrowsInvalidCommandException(string iconUrl)
    {
        var command = new EditCreditWalletCommand(
            Guid.NewGuid(),
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
