using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.RemoveCashWallet;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.CashWallets;

[TestFixture]
public class RemoveCashWalletTests : TestBase
{
    [Test]
    public async Task RemoveCashWalletCommand_Tests()
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

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveCashWalletCommand(userId, walletId));

        var removedWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(walletId, userId));

        Assert.That(removedWallet!.Id, Is.EqualTo(walletId));
        Assert.That(removedWallet.UserId, Is.EqualTo(userId));
        Assert.That(removedWallet.IsRemoved, Is.True);
    }

    [Test]
    public void RemoveCashWalletCommand_WhenWalletDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new RemoveCashWalletCommand(Guid.NewGuid(), Guid.NewGuid())),
            Throws.TypeOf<NotFoundRepositoryException<CashWallet>>());
    }

    [Test]
    public async Task RemoveCashWalletCommand_WhenWalletDoesNotBelongToUser_ThrowsAccessDeniedException()
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

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new RemoveCashWalletCommand(Guid.NewGuid(), walletId)),
            Throws.TypeOf<AccessDeniedException>());
    }
}
