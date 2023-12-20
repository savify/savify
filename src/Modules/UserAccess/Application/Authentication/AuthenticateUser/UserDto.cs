namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUser;

public class UserDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public bool IsActive { get; set; }
}
