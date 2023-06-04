namespace App.Modules.UserAccess.Application.Authentication.AuthenticateUser;

public class UserDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public bool IsActive { get; set; }
}
