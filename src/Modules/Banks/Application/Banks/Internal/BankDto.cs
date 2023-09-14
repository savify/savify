namespace App.Modules.Banks.Application.Banks.Internal;

public class BankDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string CountryCode { get; set; }

    public string ExternalProviderName { get; set; }

    public string Status { get; set; }

    public Guid LastBanksSynchronisationProcessId { get; set; }

    public int? MaxConsentDays { get; set; }

    public bool IsRegulated { get; set; }

    public string DefaultLogoUrl { get; set; }

    public string LogoUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
