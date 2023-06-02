namespace App.Modules.UserAccess.Application.Authentication;

using BCrypt.Net;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        return BCrypt.HashPassword(password, 12);
    }

    public static bool IsPasswordValid(string hashedPassword, string providedPassword)
    {
        return BCrypt.Verify(providedPassword, hashedPassword);
    }
}
