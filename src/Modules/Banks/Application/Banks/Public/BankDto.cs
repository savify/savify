namespace App.Modules.Banks.Application.Banks.Public;

public class BankDto
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public required string CountryCode { get; init; }

    public required bool IsBeta { get; init; }

    public required string DefaultLogoUrl { get; init; }

    public required string LogoUrl { get; init; }
}
