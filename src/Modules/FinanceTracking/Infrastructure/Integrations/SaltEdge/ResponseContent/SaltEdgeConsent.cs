namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ResponseContent;

public class SaltEdgeConsent
{
    public string Id { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
