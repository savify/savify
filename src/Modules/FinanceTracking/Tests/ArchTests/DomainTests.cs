using System.Reflection;
using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.ArchTests.SeedWork;

namespace App.Modules.FinanceTracking.ArchTests;

[TestFixture]
public class DomainTests : TestBase
{
    [Test]
    public void DomainEvent_ShouldBeImmutable()
    {
        var types = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEventBase))
            .Or()
            .ImplementInterface(typeof(IDomainEvent))
            .GetTypes();

        AssertAreImmutable(types);
    }

    [Test]
    public void Entity_CannotHaveReference_ToOtherAggregateRoot()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .AreNotAbstract()
            .And()
            .Inherit(typeof(Entity)).GetTypes();

        var aggregateRoots = Types.InAssembly(DomainAssembly)
            .That().ImplementInterface(typeof(IAggregateRoot)).GetTypes().ToList();

        const BindingFlags bindingFlags = BindingFlags.DeclaredOnly |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance;

        var failingTypes = new List<Type>();
        foreach (var type in entityTypes)
        {
            var fields = type.GetFields(bindingFlags);

            foreach (var field in fields)
            {
                if (aggregateRoots.Contains(field.FieldType) ||
                    field.FieldType.GenericTypeArguments.Any(x => aggregateRoots.Contains(x)))
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            var properties = type.GetProperties(bindingFlags);
            foreach (var property in properties)
            {
                if (aggregateRoots.Contains(property.PropertyType) ||
                    property.PropertyType.GenericTypeArguments.Any(x => aggregateRoots.Contains(x)))
                {
                    failingTypes.Add(type);
                    break;
                }
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Test]
    public void Entity_ShouldHaveParameterlessPrivateConstructor()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .AreNotAbstract()
            .And()
            .Inherit(typeof(Entity)).GetTypes();

        var failingTypes = new List<Type>();
        foreach (var entityType in entityTypes)
        {
            bool hasPrivateParameterlessConstructor = false;
            var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var constructorInfo in constructors)
            {
                if (constructorInfo.IsPrivate && constructorInfo.GetParameters().Length == 0)
                {
                    hasPrivateParameterlessConstructor = true;
                }
            }

            if (!hasPrivateParameterlessConstructor)
            {
                failingTypes.Add(entityType);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Test]
    public void DomainEvent_ShouldHaveDomainEventPostfix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEventBase))
            .Or()
            .ImplementInterface(typeof(IDomainEvent))
            .Should().HaveNameEndingWith("DomainEvent")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void BusinessRule_ShouldHaveRulePostfix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IBusinessRule))
            .Should().HaveNameEndingWith("Rule")
            .GetResult();

        AssertArchTestResult(result);
    }
}
