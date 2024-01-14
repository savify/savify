using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.Users.Tags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Domain.Transfers;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class EditTransfersTests : TransfersTestBase
{
    [Test]
    public async Task EditTransferCommand_EditsTransfer()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId);

        var editCommand = await CreateEditTransferCommand(transferId, userId);

        await FinanceTrackingModule.ExecuteCommandAsync(editCommand);

        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(transferId, userId));

        Assert.That(transfer, Is.Not.Null);
        Assert.That(transfer!.Id, Is.EqualTo(transferId));
        Assert.That(transfer.SourceWalletId, Is.EqualTo(editCommand.SourceWalletId));
        Assert.That(transfer.TargetWalletId, Is.EqualTo(editCommand.TargetWalletId));
        Assert.That(transfer.MadeOn, Is.EqualTo(editCommand.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(transfer.Comment, Is.EqualTo(editCommand.Comment));
        Assert.That(transfer.Tags, Is.EquivalentTo(editCommand.Tags!));
        Assert.That(transfer.SourceAmount, Is.EqualTo(editCommand.SourceAmount));
        Assert.That(transfer.SourceCurrency, Is.EqualTo("USD"));
        Assert.That(transfer.TargetAmount, Is.EqualTo(editCommand.SourceAmount));
        Assert.That(transfer.TargetCurrency, Is.EqualTo("USD"));
        Assert.That(transfer.ExchangeRate, Is.EqualTo(decimal.One));
    }

    [Test]
    public async Task EditTransferCommand_UpdatesUserTags()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId: userId);

        string[] newTags = ["New user tag 1", "New user tag 2"];
        var command = await CreateEditTransferCommand(transferId, userId, tags: newTags);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags!.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task EditTransferCommand_WhenSourceAndTargetWalletsWereNotChanged_UpdatesBalancesOnWallets()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var targetWalletId = await CreateWallet(userId, initialBalance: 100);
        var transferId = await AddNewTransferAsync(userId, sourceWalletId, targetWalletId, sourceAmount: 100);

        var command = await CreateEditTransferCommand(transferId, userId, sourceWalletId, targetWalletId, sourceAmount: 50);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var sourceWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        var targetWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));

        Assert.That(sourceWallet!.Balance, Is.EqualTo(50));
        Assert.That(targetWallet!.Balance, Is.EqualTo(150));
    }

    [Test]
    public async Task EditTransferCommand_WhenSourceWalletWasChanged_UpdatesBalancesOnWallets()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var targetWalletId = await CreateWallet(userId, initialBalance: 100);
        var transferId = await AddNewTransferAsync(userId, sourceWalletId, targetWalletId, sourceAmount: 100);

        var newSourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var command = await CreateEditTransferCommand(transferId, userId, newSourceWalletId, targetWalletId, sourceAmount: 50);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var sourceWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        var newSourceWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(newSourceWalletId, userId));
        var targetWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));

        Assert.That(sourceWallet!.Balance, Is.EqualTo(100));
        Assert.That(newSourceWallet!.Balance, Is.EqualTo(50));
        Assert.That(targetWallet!.Balance, Is.EqualTo(150));
    }

    [Test]
    public async Task EditTransferCommand_WhenTargetWalletWasChanged_UpdatesBalancesOnWallets()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var targetWalletId = await CreateWallet(userId, initialBalance: 100);
        var transferId = await AddNewTransferAsync(userId, sourceWalletId, targetWalletId, sourceAmount: 100);

        var newTargetWalletId = await CreateWallet(userId, initialBalance: 100);
        var command = await CreateEditTransferCommand(transferId, userId, sourceWalletId, newTargetWalletId, sourceAmount: 50);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var sourceWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        var targetWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));
        var newTargetWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(newTargetWalletId, userId));

        Assert.That(sourceWallet!.Balance, Is.EqualTo(50));
        Assert.That(targetWallet!.Balance, Is.EqualTo(100));
        Assert.That(newTargetWallet!.Balance, Is.EqualTo(150));
    }

    [Test]
    public async Task EditTransferCommand_WhenSourceAndTargetWalletsWereChanged_UpdatesBalancesOnWallets()
    {
        var userId = Guid.NewGuid();
        var sourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var targetWalletId = await CreateWallet(userId, initialBalance: 100);
        var transferId = await AddNewTransferAsync(userId, sourceWalletId, targetWalletId, sourceAmount: 100);

        var newSourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var newTargetWalletId = await CreateWallet(userId, initialBalance: 100);
        var command = await CreateEditTransferCommand(transferId, userId, newSourceWalletId, newTargetWalletId, sourceAmount: 50);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var sourceWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(sourceWalletId, userId));
        var newSourceWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(newSourceWalletId, userId));
        var targetWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(targetWalletId, userId));
        var newTargetWallet = await FinanceTrackingModule.ExecuteQueryAsync(new GetCashWalletQuery(newTargetWalletId, userId));

        Assert.That(sourceWallet!.Balance, Is.EqualTo(100));
        Assert.That(targetWallet!.Balance, Is.EqualTo(100));
        Assert.That(newSourceWallet!.Balance, Is.EqualTo(50));
        Assert.That(newTargetWallet!.Balance, Is.EqualTo(150));
    }

    [Test]
    public async Task EditTransferCommand_WhenTransferDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingTransferId = Guid.NewGuid();

        var command = await CreateEditTransferCommand(notExistingTransferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Transfer>>());
    }

    [Test]
    public async Task EditTransferCommand_WhenTransferForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId: userId);

        var command = await CreateEditTransferCommand(transferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenTransferIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var emptyTransferId = Guid.Empty;

        var command = await CreateEditTransferCommand(emptyTransferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var transferId = Guid.NewGuid();

        var command = await CreateEditTransferCommand(transferId, userId: OptionalParameter.Default);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenSourceWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var transferId = await AddNewTransferAsync();

        var command = await CreateEditTransferCommand(transferId, sourceWalletId: Guid.Empty);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenTargetWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var transferId = await AddNewTransferAsync();

        var command = await CreateEditTransferCommand(transferId, targetWalletId: Guid.Empty);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(-5)]
    [TestCase(0)]
    public async Task EditTransferCommand_WhenAmountIsLessOrEqualToZero_ThrowsInvalidCommandException(int amount)
    {
        var transferId = await AddNewTransferAsync();

        var command = await CreateEditTransferCommand(transferId, sourceAmount: amount);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var transferId = await AddNewTransferAsync();

        var command = await CreateEditTransferCommand(transferId, madeOn: OptionalParameter.Default);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }
}
