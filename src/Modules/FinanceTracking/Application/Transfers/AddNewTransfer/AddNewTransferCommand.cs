using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

public class AddNewTransferCommand(Guid userId, Guid sourceWalletId, Guid targetWalletId, int sourceAmount, int? targetAmount, DateTime madeOn, string? comment, IEnumerable<string>? tags)
    : CommandBase<Guid>
{
    public Guid UserId { get; } = userId;

    public Guid SourceWalletId { get; } = sourceWalletId;

    public Guid TargetWalletId { get; } = targetWalletId;

    public int SourceAmount { get; } = sourceAmount;

    public int? TargetAmount { get; } = targetAmount;

    public DateTime MadeOn { get; } = madeOn;

    public string? Comment { get; } = comment;

    public IEnumerable<string>? Tags { get; } = tags;
}
