using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.IntegrationTests.UserRegistrations;

public class UserRegistrationSampleData
{
    public static string Email = "test@email.com";

    public static string Name = "Test";
    
    public static string PlainPassword = "Test1234!";

    public static Country Country = Country.From("PL");
    
    public static Language PreferredLanguage = Language.From("en");
}
