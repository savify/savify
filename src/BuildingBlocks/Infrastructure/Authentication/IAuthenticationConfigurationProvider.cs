namespace App.BuildingBlocks.Infrastructure.Authentication;

public interface IAuthenticationConfigurationProvider
{
    public AuthenticationConfiguration GetConfiguration();
}
