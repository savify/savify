namespace App.Modules.UserAccess.Infrastructure.Authentication;

public class AuthenticationConfigurationProvider : IAuthenticationConfigurationProvider
{
    private readonly AuthenticationConfiguration _configuration;

    public AuthenticationConfigurationProvider(AuthenticationConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthenticationConfiguration GetConfiguration()
    {
        return _configuration;
    }
}
