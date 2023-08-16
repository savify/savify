namespace App.API.Modules.UserAccess.Users.Requests;

public class CreateNewUserRequest
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public string Role { get; set; }

    public string Country { get; set; }
}
