using Microsoft.EntityFrameworkCore;

namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;

public class SaltEdgeConnectionRepository : ISaltEdgeConnectionRepository
{
    private readonly FinanceTrackingContext _financeTrackingContext;

    public SaltEdgeConnectionRepository(FinanceTrackingContext financeTrackingContext)
    {
        _financeTrackingContext = financeTrackingContext;
    }

    public async Task AddAsync(SaltEdgeConnection connection)
    {
        await _financeTrackingContext.AddAsync(connection);
    }

    public async Task<SaltEdgeConnection?> GetByInternalIdAsync(Guid internalConnectionId)
    {
        return await _financeTrackingContext.SaltEdgeConnections.SingleOrDefaultAsync(x => x.InternalConnectionId == internalConnectionId);
    }
}
