namespace App.Modules.Banks.Infrastructure.Integrations.SaltEdge;

public class SaltEdgeIntegrationException : Exception
{
    public SaltEdgeIntegrationException(string? message) : base(message)
    {
    }
}
