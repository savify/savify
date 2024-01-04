using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Transfers.EditTransfer;

public class EditTransferCommand(Guid transferId, Guid userId, Guid sourceWalletId, Guid targetWalletId, int amount, string currency, DateTime madeOn, string? comment, IEnumerable<string>? tags) : CommandBase
{
    public Guid TransferId { get; } = transferId;

    public Guid UserId { get; } = userId;

    public Guid SourceWalletId { get; } = sourceWalletId;

    public Guid TargetWalletId { get; } = targetWalletId;

    public int Amount { get; } = amount;

    public string Currency { get; } = currency;

    public DateTime MadeOn { get; } = madeOn;

    public string? Comment { get; } = comment;

    public IEnumerable<string>? Tags { get; } = tags;
}
