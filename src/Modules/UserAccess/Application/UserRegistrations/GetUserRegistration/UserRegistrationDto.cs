namespace App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;

public class UserRegistrationDto
{
    public Guid Id { get; set; }

    public required string Email { get; set; }

    public required string Name { get; set; }

    public required string Status { get; set; }

    public DateTime ValidTill { get; set; }
}
