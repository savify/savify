namespace App.Modules.UserAccess.Domain.Users;

public record Language(string Value)
{
    public static Language From(string value) => new(value);
}
