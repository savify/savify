using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.GetDebitWallet;
using App.Modules.FinanceTracking.Application.Wallets.DebitWallets.RemoveDebitWallet;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.DebitWallets;

[TestFixture]
public class RemoveDebitWalletTests : TestBase
{
    [Test]
    public async Task RemoveDebitWalletCommand_Tests()
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

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveDebitWalletCommand(userId, walletId));

        var removedWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetDebitWalletQuery(walletId, userId));

        Assert.That(removedWallet!.Id, Is.EqualTo(walletId));
        Assert.That(removedWallet.UserId, Is.EqualTo(userId));
        Assert.That(removedWallet.IsRemoved, Is.True);
    }

    [Test]
    public void RemoveDebitWalletCommand_WhenWalletDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new RemoveDebitWalletCommand(Guid.NewGuid(), Guid.NewGuid())),
            Throws.TypeOf<NotFoundRepositoryException<DebitWallet>>());
    }

    [Test]
    public async Task RemoveDebitWalletCommand_WhenWalletDoesNotBelongToUser_ThrowsAccessDeniedException()
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

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new RemoveDebitWalletCommand(Guid.NewGuid(), walletId)),
            Throws.TypeOf<AccessDeniedException>());
    }
}
