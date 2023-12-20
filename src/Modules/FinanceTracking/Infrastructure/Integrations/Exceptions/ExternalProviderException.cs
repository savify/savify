namespace App.Modules.FinanceTracking.Infrastructure.Integrations.Exceptions;

public class ExternalProviderException(string? message) : Exception(message);
