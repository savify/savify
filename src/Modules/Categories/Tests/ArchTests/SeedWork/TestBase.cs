using System.Reflection;
using App.Modules.Categories.Application.Contracts;
using App.Modules.Categories.Domain.Categories;
using App.Modules.Categories.Infrastructure;

namespace App.Modules.Categories.ArchTests.SeedWork;

public abstract class TestBase
{
    protected static Assembly DomainAssembly => typeof(Category).Assembly;

    protected static Assembly ApplicationAssembly => typeof(CommandBase).Assembly;

    protected static Assembly InfrastructureAssembly => typeof(CategoriesContext).Assembly;

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
