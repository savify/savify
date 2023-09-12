namespace App.Modules.Banks.Infrastructure.Integrations.SaltEdge.Providers;

public class SaltEdgeProvider
{
    public string Code { get; }

    public string Name { get; }

    public string Status { get; }

    public string CountryCode { get; }

    public bool Regulated { get; }

    public int? MaxConsentDays { get; }

    public string LogoUrl { get; }

    public SaltEdgeProvider(string code, string name, string status, string countryCode, bool regulated, int? maxConsentDays, string logoUrl)
    {
        Code = code;
        Name = name;
        Status = status;
        CountryCode = countryCode;
        Regulated = regulated;
        MaxConsentDays = maxConsentDays;
        LogoUrl = logoUrl;
    }
}
