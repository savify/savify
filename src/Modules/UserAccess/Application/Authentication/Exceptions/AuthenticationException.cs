namespace App.Modules.UserAccess.Application.Authentication.Exceptions;

public class AuthenticationException : Exception
{
    public AuthenticationException(string? message) : base(message)
    {
    }
}
