using System.Reflection;
using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.ArchTests.SeedWork;

namespace App.Modules.UserAccess.ArchTests;

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
    public void Entity_WhichIsNotAggregateRoot_CannotHavePublicMembers()
    {
        var types = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .And().DoNotImplementInterface(typeof(IAggregateRoot)).GetTypes();

        const BindingFlags bindingFlags = BindingFlags.DeclaredOnly |
                                          BindingFlags.Public |
                                          BindingFlags.Instance |
                                          BindingFlags.Static;

        var failingTypes = new List<Type>();
        foreach (var type in types)
        {
            var publicFields = type.GetFields(bindingFlags);
            var publicProperties = type.GetProperties(bindingFlags);
            var publicMethods = type.GetMethods(bindingFlags);

            if (publicFields.Any() || publicProperties.Any() || publicMethods.Any())
            {
                failingTypes.Add(type);
            }
        }

        AssertFailingTypes(failingTypes);
    }
    
    [Test]
    public void Entity_CannotHaveReference_ToOtherAggregateRoot()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
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
