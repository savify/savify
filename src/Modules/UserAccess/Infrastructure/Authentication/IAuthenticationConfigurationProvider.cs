namespace App.Modules.UserAccess.Infrastructure.Authentication;

public interface IAuthenticationConfigurationProvider
{
    public AuthenticationConfiguration GetConfiguration();
}
