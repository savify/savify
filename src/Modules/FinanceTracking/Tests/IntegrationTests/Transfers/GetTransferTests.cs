using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class GetTransferTests : TransfersTestBase
{
    [Test]
    public async Task GetTransferQuery_WhenTransferDoesNotExist_ReturnsNull()
    {
        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(Guid.NewGuid(), Guid.NewGuid()));

        Assert.That(transfer, Is.Null);
    }

    [Test]
    public async Task GetTransferQuery_WhenTransferDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteQueryAsync(
            new GetTransferQuery(transferId, Guid.NewGuid())),
            Throws.TypeOf<AccessDeniedException>());
    }
}
