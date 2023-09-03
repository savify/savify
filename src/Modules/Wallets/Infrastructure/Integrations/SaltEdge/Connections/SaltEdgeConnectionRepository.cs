using Microsoft.EntityFrameworkCore;

namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections;

public class SaltEdgeConnectionRepository : ISaltEdgeConnectionRepository
{
    private readonly WalletsContext _walletsContext;

    public SaltEdgeConnectionRepository(WalletsContext walletsContext)
    {
        _walletsContext = walletsContext;
    }

    public async Task AddAsync(SaltEdgeConnection connection)
    {
        await _walletsContext.AddAsync(connection);
    }

    public async Task<SaltEdgeConnection?> GetByInternalIdAsync(Guid internalConnectionId)
    {
        return await _walletsContext.SaltEdgeConnections.FirstOrDefaultAsync(x => x.InternalConnectionId == internalConnectionId);
    }
}
