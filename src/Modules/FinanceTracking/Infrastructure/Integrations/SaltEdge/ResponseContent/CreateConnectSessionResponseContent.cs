namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.ResponseContent;

public class CreateConnectSessionResponseContent
{
    public DateTime ExpiresAt { get; set; }

    public required string ConnectUrl { get; set; }
}
