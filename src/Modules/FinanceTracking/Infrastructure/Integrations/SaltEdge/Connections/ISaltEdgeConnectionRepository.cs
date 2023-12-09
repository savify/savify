namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;

public interface ISaltEdgeConnectionRepository
{
    public Task AddAsync(SaltEdgeConnection connection);

    public Task<SaltEdgeConnection?> GetByInternalIdAsync(Guid internalConnectionId);
}
