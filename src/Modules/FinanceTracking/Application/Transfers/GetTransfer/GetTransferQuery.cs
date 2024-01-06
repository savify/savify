using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Transfers.GetTransfer;

public class GetTransferQuery(Guid transferId, Guid userId) : QueryBase<TransferDto?>
{
    public Guid TransferId { get; } = transferId;

    public Guid UserId { get; } = userId;
}
