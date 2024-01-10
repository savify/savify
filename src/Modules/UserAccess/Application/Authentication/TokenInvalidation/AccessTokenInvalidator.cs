namespace App.Modules.UserAccess.Application.Authentication.TokenInvalidation;

public class AccessTokenInvalidator(IInvalidatedAccessTokenRepository repository) : IAccessTokenInvalidator
{
    public async Task InvalidateAsync(string token)
    {
        await repository.AddAsync(token, DateTime.UtcNow);
    }
}
