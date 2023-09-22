namespace App.Modules.Banks.Application.Banks.Public;

public class BankDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string CountryCode { get; set; }

    public string DefaultLogoUrl { get; set; }

    public string LogoUrl { get; set; }
}
