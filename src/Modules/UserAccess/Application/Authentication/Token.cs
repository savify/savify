namespace App.Modules.UserAccess.Application.Authentication;

public record Token(string Value, DateTime Expires)
{
    public string Value { get; set; } = Value;

    public DateTime Expires { get; set; } = Expires;
}
