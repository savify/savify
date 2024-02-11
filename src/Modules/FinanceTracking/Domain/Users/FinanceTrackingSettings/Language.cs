namespace App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;

public record Language(string Value)
{
    public static Language From(string value) => new(value);
}
