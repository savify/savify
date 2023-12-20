namespace App.Modules.FinanceTracking.Infrastructure.Integrations.SaltEdge.Connections;

public class SaltEdgeConnection(
    string id,
    Guid internalConnectionId,
    string providerCode,
    string countryCode,
    string lastConsentId,
    string customerId,
    string status)
{
    public string Id { get; } = id;

    public Guid InternalConnectionId { get; set; } = internalConnectionId;

    public string ProviderCode { get; } = providerCode;

    public string CountryCode { get; } = countryCode;

    public string LastConsentId { get; } = lastConsentId;

    public string CustomerId { get; } = customerId;

    public string Status { get; } = status;
}
