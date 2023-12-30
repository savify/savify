using App.BuildingBlocks.Application.Exceptions;
using App.BuildingBlocks.Infrastructure.Exceptions;
using App.BuildingBlocks.Tests.Creating.OptionalValues;
using App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;
using App.Modules.FinanceTracking.Application.Transfers.EditTransfer;
using App.Modules.FinanceTracking.Application.Transfers.GetTransfer;
using App.Modules.FinanceTracking.Application.UserTags.GetUserTags;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.IntegrationTests.SeedWork;

namespace App.Modules.FinanceTracking.IntegrationTests.Transfers;

[TestFixture]
public partial class EditTransferTests : TestBase
{
    [Test]
    public async Task EditTransferCommand_TransferIsEdited()
    {
        var transferId = await AddNewTransferAsync();

        var command = CreateCommand(transferId);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var transfer = await FinanceTrackingModule.ExecuteQueryAsync(new GetTransferQuery(transferId));

        Assert.That(transfer, Is.Not.Null);
        Assert.That(transfer.Id, Is.EqualTo(transferId));
        Assert.That(transfer.SourceWalletId, Is.EqualTo(command.SourceWalletId));
        Assert.That(transfer.TargetWalletId, Is.EqualTo(command.TargetWalletId));
        Assert.That(transfer.Amount, Is.EqualTo(command.Amount));
        Assert.That(transfer.Currency, Is.EqualTo(command.Currency));
        Assert.That(transfer.MadeOn, Is.EqualTo(command.MadeOn).Within(TimeSpan.FromSeconds(1)));
        Assert.That(transfer.Comment, Is.EqualTo(command.Comment));
        Assert.That(transfer.Tags, Is.EquivalentTo(command.Tags!));
    }

    [Test]
    public async Task EditTransferCommand_UserTagsAreUpdated()
    {
        var userId = Guid.NewGuid();
        var transferId = await AddNewTransferAsync(userId: userId);

        string[] newTags = ["New user tag 1", "New user tag 2"];
        var command = CreateCommand(transferId, userId: userId, tags: newTags);

        await FinanceTrackingModule.ExecuteCommandAsync(command);

        var userTags = await FinanceTrackingModule.ExecuteQueryAsync(new GetUserTagsQuery(userId));

        Assert.That(userTags, Is.Not.Null);
        Assert.That(userTags.Values, Is.SupersetOf(newTags));
    }

    [Test]
    public async Task EditTransferCommand_WhenTransferDoesNotExist_ThrowsNotFoundRepositoryException()
    {
        var notExistingTransferId = Guid.NewGuid();

        var command = CreateCommand(notExistingTransferId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteCommandAsync(command), Throws.TypeOf<NotFoundRepositoryException<Transfer>>());
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
        var command = new AddNewTransferCommand(
            userId: userId.GetValueOr(Guid.NewGuid()),
            sourceWalletId: Guid.NewGuid(),
            targetWalletId: Guid.NewGuid(),
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
}
