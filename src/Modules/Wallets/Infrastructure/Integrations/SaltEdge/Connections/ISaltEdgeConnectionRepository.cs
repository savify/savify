namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections;

public interface ISaltEdgeConnectionRepository
{
    public Task AddAsync(SaltEdgeConnection connection);

    public Task<SaltEdgeConnection?> GetByInternalIdAsync(Guid internalConnectionId);
}
