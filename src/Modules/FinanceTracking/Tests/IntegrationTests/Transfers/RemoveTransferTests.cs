using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.Transfers.RemoveTransfer;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Domain.Transfers;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class RemoveTransferTests : TransfersTestBase
{
    [Test]
    public async Task RemoveTransferCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId);

        var command = new RemoveTransferCommand(transferId, userId);
        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(transferId, userId));

        Assert.That(transfer, Is.Null);
    }

    [Test]
    public async Task RemoveTransferCommand_IncreasesBalanceOnSourceWallet_And_DecreasesBalanceOnTargetWallet()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var targetWalletId = await CreateWallet(userId, initialBalance: 100);
        var transferId = await AddNewTransferAsync(userId);

        var command = new RemoveTransferCommand(transferId, userId);
        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var sourceWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        var targetWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));

        Assert.That(sourceWallet!.Balance, Is.EqualTo(100));
        Assert.That(targetWallet!.Balance, Is.EqualTo(100));
    }

    [Test]
    public async Task RemoveTransferCommand_WhenTransferIdIsDefaultGuid_ThrowsInvalidCommandException()
    {
        var command = new RemoveTransferCommand(Guid.Empty, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task RemoveTransferCommand_WhenTransferDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingTransferId = Guid.NewGuid();
        var command = new RemoveTransferCommand(notExistingTransferId, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Transfer>>());
    }

    [Test]
    public async Task RemoveTransferCommand_WhenTransferForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var transferId = await AddNewTransferAsync(Guid.NewGuid());
        var command = new RemoveTransferCommand(transferId, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }
}
