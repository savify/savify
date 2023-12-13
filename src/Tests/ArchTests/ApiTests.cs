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
            FinanceTrackingNamespace,
            BanksNamespace,
            CategoriesNamespace,
            TransactionsNamespace
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
            FinanceTrackingNamespace,
            BanksNamespace,
            CategoriesNamespace,
            TransactionsNamespace
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
            CategoriesNamespace,
            TransactionsNamespace
        };
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("App.API.Modules.FinanceTracking")
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
            FinanceTrackingNamespace,
            CategoriesNamespace,
            TransactionsNamespace
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
            FinanceTrackingNamespace,
            BanksNamespace,
            TransactionsNamespace
        };
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("App.API.Modules.Categories")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void TransactionsApi_DoesNotHaveDependency_ToOtherModules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace,
            NotificationsNamespace,
            FinanceTrackingNamespace,
            BanksNamespace,
            CategoriesNamespace
        };
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespace("App.API.Modules.Transactions")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }
}
