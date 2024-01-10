﻿using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.EditTransfer;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.GetCashWallet;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class EditTransferTests : TestBase
{
    [Test]
    public async Task EditTransferCommand_EditsTransfer()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId);

        var editCommand = await CreateCommand(transferId, userId);

        await FinanceTrackingModule.ExecuteCommandAsync(editCommand);

        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(transferId, userId));

        Assert.That(transfer, Is.Not.Null);
        Assert.That(transfer!.Id, Is.EqualTo(transferId));
        Assert.That(transfer.SourceWalletId, Is.EqualTo(editCommand.SourceWalletId));
        Assert.That(transfer.TargetWalletId, Is.EqualTo(editCommand.TargetWalletId));
        Assert.That(transfer.Amount, Is.EqualTo(editCommand.Amount));
        Assert.That(transfer.Currency, Is.EqualTo(editCommand.Currency));
        Assert.That(transfer.MadeOn, Is.EqualTo(editCommand.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(transfer.Comment, Is.EqualTo(editCommand.Comment));
        Assert.That(transfer.Tags, Is.EquivalentTo(editCommand.Tags!));
    }

    [Test]
    public async Task EditTransferCommand_UpdatesUserTags()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId: userId);

        string[] newTags = ["New user tag 1", "New user tag 2"];
        var command = await CreateCommand(transferId, userId, tags: newTags);

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
        var transferId = await AddNewTransferAsync(userId, sourceWalletId, targetWalletId, amount: 100);

        var command = await CreateCommand(transferId, userId, sourceWalletId, targetWalletId, amount: 50);

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
        var transferId = await AddNewTransferAsync(userId, sourceWalletId, targetWalletId, amount: 100);

        var newSourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var command = await CreateCommand(transferId, userId, newSourceWalletId, targetWalletId, amount: 50);

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
        var transferId = await AddNewTransferAsync(userId, sourceWalletId, targetWalletId, amount: 100);

        var newTargetWalletId = await CreateWallet(userId, initialBalance: 100);
        var command = await CreateCommand(transferId, userId, sourceWalletId, newTargetWalletId, amount: 50);

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
        var transferId = await AddNewTransferAsync(userId, sourceWalletId, targetWalletId, amount: 100);

        var newSourceWalletId = await CreateWallet(userId, initialBalance: 100);
        var newTargetWalletId = await CreateWallet(userId, initialBalance: 100);
        var command = await CreateCommand(transferId, userId, newSourceWalletId, newTargetWalletId, amount: 50);

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

        var command = await CreateCommand(notExistingTransferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Transfer>>());
    }

    [Test]
    public async Task EditTransferCommand_WhenTransferForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId: userId);

        var command = await CreateCommand(transferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenTransferIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var emptyTransferId = Guid.Empty;

        var command = await CreateCommand(emptyTransferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var transferId = Guid.NewGuid();

        var command = await CreateCommand(transferId, userId: OptionalParameter.Default);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenSourceWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var transferId = await AddNewTransferAsync();

        var command = await CreateCommand(transferId, sourceWalletId: Guid.Empty);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenTargetWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var transferId = await AddNewTransferAsync();

        var command = await CreateCommand(transferId, targetWalletId: Guid.Empty);

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

        var command = await CreateCommand(transferId, amount: amount);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    [TestCase(null!)]
    [TestCase("")]
    [TestCase("PL")]
    [TestCase("PLNN")]
    public async Task EditTransferCommand_WhenCurrencyIsNotIsoFormat_ThrowsInvalidCommandException(string currency)
    {
        var transferId = await AddNewTransferAsync();

        var command = await CreateCommand(transferId, currency: currency);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var transferId = await AddNewTransferAsync();

        var command = await CreateCommand(transferId, madeOn: OptionalParameter.Default);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    private async Task<Guid> AddNewTransferAsync(
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> sourceWalletId = default,
        OptionalParameter<Guid> targetWalletId = default,
        OptionalParameter<int> amount = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());
        var sourceWalletIdValue = sourceWalletId.GetValueOr(await CreateWallet(userIdValue));
        var targetWalletIdValue = targetWalletId.GetValueOr(await CreateWallet(userIdValue));

        var command = new AddNewTransferCommand(
            userId: userIdValue,
            sourceWalletId: sourceWalletIdValue,
            targetWalletId: targetWalletIdValue,
            amount: amount.GetValueOr(100),
            currency: "USD",
            madeOn: DateTime.UtcNow,
            comment: "Savings transfer",
            tags: ["Savings", "Minor"]);

        var transferId = await FinanceTrackingModule.ExecuteCommandAsync(command);

        return transferId;
    }

    private async Task<EditTransferCommand> CreateCommand(
        Guid transferId,
        OptionalParameter<Guid> userId = default,
        OptionalParameter<Guid> sourceWalletId = default,
        OptionalParameter<Guid> targetWalletId = default,
        OptionalParameter<int> amount = default,
        OptionalParameter<string> currency = default,
        OptionalParameter<DateTime> madeOn = default,
        OptionalParameter<string> comment = default,
        OptionalParameter<IEnumerable<string>> tags = default)
    {
        var userIdValue = userId.GetValueOr(Guid.NewGuid());

        return new EditTransferCommand(
            transferId,
            userIdValue,
            sourceWalletId.GetValueOr(await CreateWallet(userIdValue)),
            targetWalletId.GetValueOr(await CreateWallet(userIdValue)),
            amount.GetValueOr(500),
            currency.GetValueOr("PLN"),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Edited transfer"),
            tags.GetValueOr(["Edited"]));
    }

    private async Task<Guid> CreateWallet(Guid userId, int initialBalance = 100)
    {
        return await FinanceTrackingModule.ExecuteCommandAsync(new AddNewCashWalletCommand(
            userId.Equals(Guid.Empty) ? Guid.NewGuid() : userId,
            "Cash wallet",
            "USD",
            initialBalance,
            "#000000",
            "https://cdn.savify.io/icons/icon.svg",
            true));
    }
}
