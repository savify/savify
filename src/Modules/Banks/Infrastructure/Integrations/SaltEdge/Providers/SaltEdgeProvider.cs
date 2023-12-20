namespace App.Modules.Banks.Infrastructure.Integrations.SaltEdge.Providers;

public class SaltEdgeProvider(
    string code,
    string name,
    string status,
    string countryCode,
    bool regulated,
    int? maxConsentDays,
    string logoUrl)
{
    public string Code { get; } = code;

    public string Name { get; } = name;

    public string Status { get; } = status;

    public string CountryCode { get; } = countryCode;

    public bool Regulated { get; } = regulated;

    public int? MaxConsentDays { get; } = maxConsentDays;

    public string LogoUrl { get; } = logoUrl;
}
