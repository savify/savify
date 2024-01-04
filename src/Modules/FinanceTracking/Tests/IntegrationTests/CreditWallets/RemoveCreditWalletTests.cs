using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.GetCreditWallet;
using App.Modules.FinanceTracking.Application.Wallets.CreditWallets.RemoveCreditWallet;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.CreditWallets;

[TestFixture]
public class RemoveCreditWalletTests : TestBase
{
    [Test]
    public async Task RemoveCreditWalletCommand_Tests()
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

        await FinanceTrackingModule.ExecuteCommandAsync(new RemoveCreditWalletCommand(userId, walletId));

        var removedWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCreditWalletQuery(walletId, userId));

        Assert.That(removedWallet!.Id, Is.EqualTo(walletId));
        Assert.That(removedWallet.UserId, Is.EqualTo(userId));
        Assert.That(removedWallet.IsRemoved, Is.True);
    }

    [Test]
    public void RemoveCreditWalletCommand_WhenWalletDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new RemoveCreditWalletCommand(Guid.NewGuid(), Guid.NewGuid())),
            Throws.TypeOf<NotFoundRepositoryException<CreditWallet>>());
    }

    [Test]
    public async Task RemoveCreditWalletCommand_WhenWalletDoesNotBelongToUser_ThrowsAccessDeniedException()
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

        Assert.That(() => FinanceTrackingModule.ExecuteCommandAsync(new RemoveCreditWalletCommand(Guid.NewGuid(), walletId)),
            Throws.TypeOf<AccessDeniedException>());
    }
}
