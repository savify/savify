namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;

public class SaltEdgeConnection
{
    public string Id { get; }

    public Guid InternalConnectionId { get; set; }

    public string ProviderCode { get; }

    public string CountryCode { get; }

    public string LastConsentId { get; }

    public string CustomerId { get; }

    public string Status { get; }

    public SaltEdgeConnection(string id, Guid internalConnectionId, string providerCode, string countryCode, string lastConsentId, string customerId, string status)
    {
        Id = id;
        InternalConnectionId = internalConnectionId;
        ProviderCode = providerCode;
        CountryCode = countryCode;
        LastConsentId = lastConsentId;
        CustomerId = customerId;
        Status = status;
    }
}
