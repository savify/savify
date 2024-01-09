namespace App.Modules.UserAccess.Application.Authentication.TokenInvalidation;

public interface IAccessTokenInvalidator
{
    Task InvalidateAsync(string token);
}
