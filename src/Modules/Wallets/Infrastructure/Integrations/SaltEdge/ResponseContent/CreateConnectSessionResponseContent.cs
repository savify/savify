namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.ResponseContent;

public class CreateConnectSessionResponseContent
{
    public DateTime ExpiresAt { get; set; }

    public string ConnectUrl { get; set; }
}
