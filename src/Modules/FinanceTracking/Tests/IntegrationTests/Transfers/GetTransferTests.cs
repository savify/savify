using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class GetTransferTests : TestBase
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

    private async Task<Guid> AddNewTransferAsync(OptionalParameter<Guid> userId = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());
        var sourceWalletId = await CreateWallet(userIdValue);
        var targetWalletId = await CreateWallet(userIdValue);

        var command = new AddNewTransferCommand(
            userId: userIdValue,
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
