using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalParameters;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.EditTransfer;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
using App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public class EditTransferTests : TestBase
{
    [Test]
    public async Task EditTransferCommand_TransferIsEdited()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId);

        var newSourceWalletId = await CreateWallet(userId);
        var newTargetWalletId = await CreateWallet(userId);

        var editCommand = CreateCommand(transferId, userId, newSourceWalletId, newTargetWalletId);

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
    public async Task EditTransferCommand_UserTagsAreUpdated()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId: userId);

        var newSourceWalletId = await CreateWallet(userId);
        var newTargetWalletId = await CreateWallet(userId);

        string[] newTags = ["New user tag 1", "New user tag 2"];
        var command = CreateCommand(transferId, userId, newSourceWalletId, newTargetWalletId, tags: newTags);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags!.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task EditTransferCommand_WhenTransferDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingTransferId = Guid.NewGuid();

        var command = CreateCommand(notExistingTransferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Transfer>>());
    }

    [Test]
    public async Task EditTransferCommand_WhenTransferForUserIdDoesNotExist_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId: userId);

        var command = CreateCommand(transferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<AccessDeniedException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenTransferIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var emptyTransferId = Guid.Empty;

        var command = CreateCommand(emptyTransferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenUserIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var transferId = Guid.NewGuid();

        var command = CreateCommand(transferId, userId: OptionalParameter.Default);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenSourceWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var transferId = await AddNewTransferAsync();

        var command = CreateCommand(transferId, sourceWalletId: Guid.Empty);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenTargetWalletIdIsEmptyGuid_ThrowsInvalidCommandException()
    {
        var transferId = await AddNewTransferAsync();

        var command = CreateCommand(transferId, targetWalletId: Guid.Empty);

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

        var command = CreateCommand(transferId, amount: amount);

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

        var command = CreateCommand(transferId, currency: currency);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
    }

    [Test]
    public async Task EditTransferCommand_WhenMadeOnIsDefaultDate_ThrowsInvalidCommandException()
    {
        var transferId = await AddNewTransferAsync();

        var command = CreateCommand(transferId, madeOn: OptionalParameter.Default);

        await Assert.ThatAsync(
            () => FinanceTrackingModule.ExecuteCommandAsync(command),
            Throws.TypeOf<InvalidCommandException>());
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

    private EditTransferCommand CreateCommand(
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
        return new EditTransferCommand(
            transferId,
            userId.GetValueOr(Guid.NewGuid()),
            sourceWalletId.GetValueOr(Guid.NewGuid()),
            targetWalletId.GetValueOr(Guid.NewGuid()),
            amount.GetValueOr(500),
            currency.GetValueOr("PLN"),
            madeOn.GetValueOr(DateTime.UtcNow),
            comment.GetValueOr("Edited transfer"),
            tags.GetValueOr(["Edited"]));
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
