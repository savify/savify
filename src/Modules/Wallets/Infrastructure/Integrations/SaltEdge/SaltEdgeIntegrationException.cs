namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge;

public class SaltEdgeIntegrationException : Exception
{
    public SaltEdgeIntegrationException(string? message) : base(message)
    {
    }
}
