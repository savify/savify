using System.Text.Json.Serialization;

namespace App.API.Modules.Wallets.Integrations.SaltEdge;

public class SaltEdgeCallbackRequest
{
    public SaltEdgeCallbackRequestData Data { get; set; }
    
    public SaltEdgeCallbackRequestMeta Meta { get; set; }
    
    public class SaltEdgeCallbackRequestData
    {
        [JsonPropertyName("connection_id")]
        public string ConnectionId { get; set; }

        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; }

        public string Stage { get; set; }
        
        [JsonPropertyName("custom_fields")]
        public SaltEdgeCallbackCustomFields CustomFields { get; set; }
    }
    
    public class SaltEdgeCallbackRequestMeta
    {
        public string Version { get; set; }
        
        public DateTime Time { get; set; }
    }
    
    public class SaltEdgeCallbackCustomFields
    {
        [JsonPropertyName("bank_connection_process_id")]
        public Guid BankConnectionProcessId { get; set; }
    }
}
