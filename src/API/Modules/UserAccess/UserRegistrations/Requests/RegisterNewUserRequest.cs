namespace App.API.Modules.UserAccess.UserRegistrations.Requests;

public class RegisterNewUserRequest
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public string Country { get; set; }
}
