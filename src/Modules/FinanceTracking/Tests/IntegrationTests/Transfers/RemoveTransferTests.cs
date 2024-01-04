using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.Transfers.RemoveTransfer;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class RemoveTransferTests : TestBase
{
    [Test]
    public async Task RemoveTransferCommand_Tests()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId);

        var command = new RemoveTransferCommand(transferId, userId);
        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(transferId));

        Assert.That(transfer, Is.Null);
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
    public async Task RemoveTransferCommand_WhenTransferForUserIdDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var transferId = await AddNewTransferAsync(Guid.NewGuid());
        var command = new RemoveTransferCommand(transferId, Guid.NewGuid());

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Transfer>>());
    }

    private async Task<Guid> AddNewTransferAsync(Guid userId)
    {
        var sourceWalletId = await CreateWallet(userId);
        var targetWalletId = await CreateWallet(userId);

        var command = new AddNewTransferCommand(
            userId: userId,
            sourceWalletId: sourceWalletId,
            targetWalletId: targetWalletId,
            amount: 100,
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var transferId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return transferId;
    }

    private async Task<Guid> CreateWallet(Guid userId)
    {
        return await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId,
            "Cash wallet",
            "USD",
            100,
            "#000000",
            "https://cdn.savify.io/icons/icon.svg",
            true));
    }
}
