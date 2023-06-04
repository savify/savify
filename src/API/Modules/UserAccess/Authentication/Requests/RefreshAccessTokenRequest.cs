namespace App.API.Modules.UserAccess.Authentication.Requests;

public class RefreshAccessTokenRequest
{
    public Guid UserId { get; set; }
    
    public string RefreshToken { get; set; }
}
