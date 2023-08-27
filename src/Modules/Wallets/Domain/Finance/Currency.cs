namespace App.Modules.Wallets.Domain.Finance;

public record Currency(string Value)
{
    public static Currency From(string value)
    {
        return new Currency(value);
    }
}
