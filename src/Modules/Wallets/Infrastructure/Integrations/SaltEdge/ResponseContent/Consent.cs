namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent;

public class Consent
{
    public string Id { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
}
