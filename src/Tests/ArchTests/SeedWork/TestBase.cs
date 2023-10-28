using System.Reflection;
using App.API.Configuration.ExecutionContext;

namespace App.ArchTests.SeedWork;

public abstract class TestBase
{
    protected static Assembly ApiAssembly => typeof(ExecutionContextAccessor).Assembly;

    public const string UserAccessNamespace = "App.Modules.UserAccess";

    public const string NotificationsNamespace = "App.Modules.Notifications";

    public const string WalletsNamespace = "App.Modules.Wallets";

    public const string BanksNamespace = "App.Modules.Banks";

    public const string CategoriesNamespace = "App.Modules.Categories";

    public const string TransactionsNamespace = "App.Modules.Transactions";

    protected static void AssertAreImmutable(IEnumerable<Type> types)
    {
        IList<Type> failingTypes = new List<Type>();
        foreach (var type in types)
        {
            if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
            {
                failingTypes.Add(type);
                break;
            }
        }

        AssertFailingTypes(failingTypes);
    }

    protected static void AssertFailingTypes(IEnumerable<Type> types)
    {
        Assert.That(types, Is.Null.Or.Empty);
    }

    protected static void AssertArchTestResult(TestResult result)
    {
        AssertFailingTypes(result.FailingTypes);
    }
}
