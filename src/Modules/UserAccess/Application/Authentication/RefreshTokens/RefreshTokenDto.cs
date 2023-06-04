namespace App.Modules.UserAccess.Application.Authentication;

public class RefreshTokenDto
{
    public Guid UserId { get; set; }
    
    public string Value { get; set; }
    
    public DateTime ExpiresAt { get; set; }
}
