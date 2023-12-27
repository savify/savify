using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Transfers.RemoveTransfer;
public class RemoveTransferCommand(Guid transferId) : CommandBase
{
    public Guid TransferId { get; } = transferId;
}
