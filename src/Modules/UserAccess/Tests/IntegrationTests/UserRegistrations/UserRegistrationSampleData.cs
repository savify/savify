using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.IntegrationTests.UserRegistrations;

public class UserRegistrationSampleData
{
    public static string Email = "test@email.com";

    public static string Name = "Test";
    
    public static string PlainPassword = "password";

    public static Language PreferredLanguage = Language.From("en");
}
