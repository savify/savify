namespace App.Modules.UserAccess.Application.Users.GetUsers;

public class UserDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string[] Roles { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
