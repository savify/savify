using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.IntegrationTests.Users;

public class UserSampleData
{
    public static string Email = "test@email.com";

    public static string Name = "Test";
    
    public static string PlainPassword = "password";

    public static UserRole Role = UserRole.User;
}
