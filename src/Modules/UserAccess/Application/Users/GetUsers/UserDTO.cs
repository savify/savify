namespace App.Modules.UserAccess.Application.Users.GetUsers;

public class UserDTO
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string Role { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
