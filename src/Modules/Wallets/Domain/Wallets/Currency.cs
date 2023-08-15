namespace App.Modules.Wallets.Domain.Wallets;

public record Currency(string Value)
{
    public static Currency From(string value)
    {
        return new Currency(value);
    }
}
