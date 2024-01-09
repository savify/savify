namespace App.Modules.UserAccess.Application.Authentication.TokenInvalidation;

public interface IInvalidatedAccessTokenRepository
{
    public Task AddAsync(string token, DateTime invalidatedAt);

    public Task<bool> IsInvalidatedAsync(string value);
}
