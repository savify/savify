namespace App.Modules.Banks.Domain.ExternalProviders;

public record ExternalProviderName(string Value)
{
    public static readonly ExternalProviderName SaltEdge = new(nameof(SaltEdge));
}
