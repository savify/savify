using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.Transfers.RemoveTransfer;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class RemoveTransferTests : TestBase
{
    [Test]
    public async Task RemoveTransferCommand_Tests()
    {
        var transferId = await AddNewTransferAsync();

        var command = new RemoveTransferCommand(transferId);
        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(transferId));

        Assert.That(transfer, Is.Null);
    }

    [Test]
    public async Task RemoveTranferCommand_WhenTransferIdIsDefaultGuid_ThrowsInvalidCommandException()
    {
        var command = new RemoveTransferCommand(Guid.Empty);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task RemoveTransferCommand_WhenTransferDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingTransferId = Guid.NewGuid();
        var command = new RemoveTransferCommand(notExistingTransferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Transfer>>());
    }

    private async Task<Guid> AddNewTransferAsync()
    {
        var command = new AddNewTransferCommand(
            sourceWalletId: Guid.NewGuid(),
            targetWalletId: Guid.NewGuid(),
            amount: 100,
            currency: "USD",
            categoryId: Guid.NewGuid(),
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var transferId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return transferId;
    }
}
