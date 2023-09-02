using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnectionProcessing;

public class BankConnectionProcessRepository : IBankConnectionProcessRepository
{
    private readonly WalletsContext _walletsContext;

    public BankConnectionProcessRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(BankConnectionProcess bankConnectionProcess)
    {
        await _walletsContext.AddAsync(bankConnectionProcess);
    }

    public async Task<BankConnectionProcess> GetByIdAsync(BankConnectionProcessId id)
    {
        var bankConnectionProcess = await _walletsContext.BankConnectionProcesses.FirstOrDefaultAsync(x => x.Id == id);

        if (bankConnectionProcess == null)
        {
            throw new NotFoundRepositoryException<BankConnectionProcess>(id.Value);
        }

        return bankConnectionProcess;
    }

    public async Task<BankConnectionProcess> GetByIdAndUserIdAsync(BankConnectionProcessId id, UserId userId)
    {
        var bankConnectionProcess = await _walletsContext.BankConnectionProcesses.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (bankConnectionProcess == null)
        {
            throw new NotFoundRepositoryException<BankConnectionProcess>(
                "Bank connection process with id '{0}' was not found for user with id '{1}'",
                new object[] { id.Value, userId.Value });
        }

        return bankConnectionProcess;
    }
}
