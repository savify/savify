namespace App.Modules.Banks.Domain.ExternalProviders;

public record ExternalProviderName(string Value)
{
    public static ExternalProviderName SaltEdge => new(nameof(SaltEdge));
}
