using System.Reflection;
using App.API.Configuration.ExecutionContext;

namespace App.ArchTests.SeedWork;

public abstract class TestBase
{
    protected static Assembly ApiAssembly => typeof(ExecutionContextAccessor).Assembly;

    protected const string UserAccessNamespace = "App.Modules.UserAccess";

    protected const string NotificationsNamespace = "App.Modules.Notifications";

    protected const string FinanceTrackingNamespace = "App.Modules.FinanceTracking";

    protected const string BanksNamespace = "App.Modules.Banks";

    protected const string CategoriesNamespace = "App.Modules.Categories";

    protected const string TransactionsNamespace = "App.Modules.Transactions";

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
