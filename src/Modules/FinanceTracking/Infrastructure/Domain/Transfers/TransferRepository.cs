using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Transfers;

internal class TransferRepository(FinanceTrackingContext context) : ITransferRepository
{
    public async Task AddAsync(Transfer transfer)
    {
        await context.AddAsync(transfer);
    }

    public async Task<Transfer> GetByIdAsync(TransferId id)
    {
        var transfer = await context.Transfers.SingleOrDefaultAsync(t => t.Id == id);

        if (transfer is null)
        {
            throw new NotFoundRepositoryException<Transfer>(id.Value);
        }

        return transfer;
    }

    public async Task<Transfer> GetByIdAndUserIdAsync(TransferId id, UserId userId)
    {
        var transfer = await context.Transfers.SingleOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (transfer is null)
        {
            throw new NotFoundRepositoryException<Transfer>(
                "Wallet with id '{0}' was not found for user with id '{1}'",
                [id.Value, userId.Value]);
        }

        return transfer;
    }

    public void Remove(Transfer transfer)
    {
        context.Remove(transfer);
    }
}
