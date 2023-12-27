using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.FinanceTracking.Domain.Transfers;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Transfers;
internal class TransfersRepository : ITransfersRepository
{
    private readonly FinanceTrackingContext _context;

    public TransfersRepository(FinanceTrackingContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transfer transfer)
    {
        await _context.AddAsync(transfer);
    }

    public async Task<Transfer> GetByIdAsync(TransferId id)
    {
        var transfer = await _context.Transfers.SingleOrDefaultAsync(t => t.Id == id);

        if (transfer is null)
        {
            throw new NotFoundRepositoryException<Transfer>(id.Value);
        }

        return transfer;
    }

    public void Remove(Transfer transfer)
    {
        _context.Remove(transfer);
    }
}
