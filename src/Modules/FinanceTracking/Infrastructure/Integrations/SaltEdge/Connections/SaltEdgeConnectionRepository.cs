using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;

public class SaltEdgeConnectionRepository(FinanceTrackingContext financeTrackingContext) : ISaltEdgeConnectionRepository
{
    public async Task AddAsync(SaltEdgeConnection connection)
    {
        await financeTrackingContext.AddAsync(connection);
    }

    public async Task<SaltEdgeConnection?> GetByInternalIdAsync(Guid internalConnectionId)
    {
        return await financeTrackingContext.SaltEdgeConnections.SingleOrDefaultAsync(x => x.InternalConnectionId == internalConnectionId);
    }
}
