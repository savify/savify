using System.Security.Cryptography;

namespace App.Modules.UserAccess.Domain.UserRegistrations;

public record ConfirmationCode(string Value)
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    private const int Length = 6;
    
    public static ConfirmationCode Generate()
    {
        var value = new String(Enumerable.Repeat(Alphabet, Length).Select(s => s[RandomNumberGenerator.GetInt32(s.Length)]).ToArray());
     
        return new ConfirmationCode(value);
    }

    public static ConfirmationCode From(string value)
    {
        return new ConfirmationCode(value);
    }
}
