using System.Reflection;
using App.BuildingBlocks.Application.Validators;
using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Configuration.Queries;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.ArchTests.SeedWork;
using MediatR;
using Newtonsoft.Json;

namespace App.Modules.Notifications.ArchTests;

[TestFixture]
public class ApplicationTests : TestBase
{
    [Test]
    public void Command_ShouldBeImmutable()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(CommandBase))
            .Or()
            .Inherit(typeof(CommandBase<>))
            .Or()
            .Inherit(typeof(InternalCommandBase<>))
            .Or()
            .Inherit(typeof(InternalCommandBase<>))
            .Or()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .GetTypes();

        AssertAreImmutable(types);
    }

    [Test]
    public void Query_ShouldBeImmutable()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That().ImplementInterface(typeof(IQuery<>)).GetTypes();

        AssertAreImmutable(types);
    }
    
    [Test]
    public void CommandHandler_ShouldHaveName_EndingWithCommandHandler()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .And()
            .DoNotHaveNameMatching(".*Decorator.*").Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        AssertArchTestResult(result);
    }
    
    [Test]
    public void QueryHandler_ShouldHaveName_EndingWithQueryHandler()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void CommandAndQueryHandlers_ShouldNotBePublic()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                    .Or()
                .ImplementInterface(typeof(ICommandHandler<>))
                    .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
            .Should().NotBePublic().GetResult().FailingTypes;

        AssertFailingTypes(types);
    }

    [Test]
    public void Validator_ShouldHaveName_EndingWithValidator()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(Validator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void Validators_ShouldNotBePublic()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(Validator<>))
            .Should().NotBePublic().GetResult().FailingTypes;

        AssertFailingTypes(types);
    }

    [Test]
    public void InternalCommand_ShouldHaveJsonConstructorAttribute()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(InternalCommandBase<>))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach (var type in types)
        {
            bool hasJsonConstructorDefined = false;
            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            foreach (var constructorInfo in constructors)
            {
                var jsonConstructorAttribute = constructorInfo.GetCustomAttributes(typeof(JsonConstructorAttribute), false);
                if (jsonConstructorAttribute.Length > 0)
                {
                    hasJsonConstructorDefined = true;
                    break;
                }
            }

            if (!hasJsonConstructorDefined)
            {
                failingTypes.Add(type);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Test]
    public void MediatR_RequestHandler_ShouldNotBeUsedDirectly()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That().DoNotHaveName("ICommandHandler`1")
            .Should().ImplementInterface(typeof(IRequestHandler<>))
            .GetTypes();

        List<Type> failingTypes = new List<Type>();
        foreach (var type in types)
        {
            bool isCommandHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
            bool isCommandWithResultHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
            bool isQueryHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
            if (!isCommandHandler && !isCommandWithResultHandler && !isQueryHandler)
            {
                failingTypes.Add(type);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Test]
    public void Command_WithResult_ShouldNotReturnUnit()
    {
        Type commandWithResultHandlerType = typeof(ICommandHandler<,>);
        IEnumerable<Type> types = Types.InAssembly(ApplicationAssembly)
            .That().ImplementInterface(commandWithResultHandlerType)
            .GetTypes().ToList();

        var failingTypes = new List<Type>();
        foreach (Type type in types)
        {
            Type interfaceType = type.GetInterface(commandWithResultHandlerType.Name);
            if (interfaceType?.GenericTypeArguments[1] == typeof(Unit))
            {
                failingTypes.Add(type);
            }
        }

        AssertFailingTypes(failingTypes);
    }
}
