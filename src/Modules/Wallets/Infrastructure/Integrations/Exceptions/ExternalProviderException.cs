namespace App.Modules.Wallets.Infrastructure.Integrations.Exceptions;

public class ExternalProviderException : Exception
{
    public ExternalProviderException(string? message) : base(message)
    {
    }
}
