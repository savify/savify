using App.ArchTests.SeedWork;

namespace App.ArchTests;

[TestFixture]
public class ApiTests : TestBase
{
    [Test]
    public void UserAccessApi_DoesNotHaveDependency_ToOtherModules()
    {
        var otherModules = new List<string>
        {
            NotificationsNamespace
        };
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("App.API.Modules.UserAccess")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }
    
    [Test]
    public void NotificationsApi_DoesNotHaveDependency_ToOtherModules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace
        };
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("DensMed.API.Modules.Notifications")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }
}