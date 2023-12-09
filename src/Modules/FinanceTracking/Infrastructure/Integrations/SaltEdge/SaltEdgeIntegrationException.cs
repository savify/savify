namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge;

public class SaltEdgeIntegrationException : Exception
{
    public SaltEdgeIntegrationException(string? message) : base(message)
    {
    }
}
