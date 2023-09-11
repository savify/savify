using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.Wallets.Domain.BankConnections;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnections;

public class BankConnectionRepository : IBankConnectionRepository
{
    private readonly WalletsContext _walletsContext;

    public BankConnectionRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(BankConnection bankConnection)
    {
        await _walletsContext.AddAsync(bankConnection);
    }

    public async Task<BankConnection> GetByIdAsync(BankConnectionId id)
    {
        var bankConnection = _walletsContext.BankConnections.Local.SingleOrDefault(x => x.Id == id) ??
                             await _walletsContext.BankConnections.SingleOrDefaultAsync(x => x.Id == id);

        if (bankConnection == null)
        {
            throw new NotFoundRepositoryException<BankConnection>(id.Value);
        }

        return bankConnection;
    }

    public void Remove(BankConnection bankConnection)
    {
        _walletsContext.Remove(bankConnection);
    }
}
