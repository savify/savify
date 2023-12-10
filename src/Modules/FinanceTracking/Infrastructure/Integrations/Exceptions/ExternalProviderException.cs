namespace App.Modules.FinanceTracking.Infrastructure.Integrations.Exceptions;

public class ExternalProviderException : Exception
{
    public ExternalProviderException(string? message) : base(message)
    {
    }
}
