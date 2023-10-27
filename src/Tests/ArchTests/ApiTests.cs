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
            NotificationsNamespace,
            WalletsNamespace,
            BanksNamespace,
            CategoriesNamespace
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
            UserAccessNamespace,
            WalletsNamespace,
            BanksNamespace,
            CategoriesNamespace
        };
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("App.API.Modules.Notifications")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void WalletsApi_DoesNotHaveDependency_ToOtherModules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace,
            NotificationsNamespace,
            BanksNamespace,
            CategoriesNamespace
        };
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("App.API.Modules.Wallets")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void BanksApi_DoesNotHaveDependency_ToOtherModules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace,
            NotificationsNamespace,
            WalletsNamespace,
            CategoriesNamespace
        };
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("App.API.Modules.Banks")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void CategoriesApi_DoesNotHaveDependency_ToOtherModules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace,
            NotificationsNamespace,
            WalletsNamespace,
            BanksNamespace
        };
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("App.API.Modules.Categories")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }
}
