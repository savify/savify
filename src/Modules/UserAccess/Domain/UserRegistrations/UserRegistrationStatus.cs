namespace App.Modules.UserAccess.Domain.UserRegistrations;

public record UserRegistrationStatus(string Value)
{
    public static UserRegistrationStatus WaitingForConfirmation => new(nameof(WaitingForConfirmation));
    
    public static UserRegistrationStatus Confirmed => new(nameof(Confirmed));
    
    public static UserRegistrationStatus Expired => new(nameof(Expired));
}