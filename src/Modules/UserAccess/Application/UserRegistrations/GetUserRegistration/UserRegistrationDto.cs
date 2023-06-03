namespace App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;

public class UserRegistrationDto
{
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public string Name { get; set; }
    
    public string Status { get; set; }
    
    public DateTime ValidTill { get; set; }
}
