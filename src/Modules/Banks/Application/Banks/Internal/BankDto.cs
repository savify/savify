namespace App.Modules.Banks.Application.Banks.Internal;

public class BankDto
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public required string CountryCode { get; init; }

    public required string ExternalProviderName { get; init; }

    public required string Status { get; init; }

    public Guid LastBanksSynchronisationProcessId { get; init; }

    public int? MaxConsentDays { get; init; }

    public bool IsRegulated { get; init; }

    public required string DefaultLogoUrl { get; init; }

    public required string LogoUrl { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
