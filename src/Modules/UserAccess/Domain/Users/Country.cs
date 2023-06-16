namespace App.Modules.UserAccess.Domain.Users;

public record Country(string Value)
{
    public static Country From(string value) => new(value);
}
