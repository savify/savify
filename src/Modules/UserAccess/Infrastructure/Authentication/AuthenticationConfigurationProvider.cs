namespace App.Modules.UserAccess.Infrastructure.Authentication;

public class AuthenticationConfigurationProvider(AuthenticationConfiguration configuration) : IAuthenticationConfigurationProvider
{
    public AuthenticationConfiguration GetConfiguration()
    {
        return configuration;
    }
}
