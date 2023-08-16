using System.Reflection;
using App.ArchTests.SeedWork;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using App.Modules.Notifications.Infrastructure;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Infrastructure;
using MediatR;

namespace App.ArchTests;

[TestFixture]
public class ModulesTests : TestBase
{
    [Test]
    public void UserAccessModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        var otherModules = new List<string>
        {
            NotificationsNamespace
        };
        List<Assembly> userAccessAssemblies = new List<Assembly>
        {
            typeof(IUserAccessModule).Assembly,
            typeof(User).Assembly,
            typeof(UserAccessContext).Assembly
        };

        var result = Types.InAssemblies(userAccessAssemblies)
            .That()
            .DoNotImplementInterface(typeof(INotificationHandler<>))
            .And().DoNotHaveNameEndingWith("IntegrationEventHandler")
            .And().DoNotHaveName("EventBusInitialization")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }

    [Test]
    public void NotificationsModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace
        };
        List<Assembly> notificationAssemblies = new List<Assembly>
        {
            typeof(INotificationsModule).Assembly,
            typeof(UserNotificationSettings).Assembly,
            typeof(NotificationsContext).Assembly
        };

        var result = Types.InAssemblies(notificationAssemblies)
            .That()
            .DoNotImplementInterface(typeof(INotificationHandler<>))
            .And().DoNotHaveNameEndingWith("IntegrationEventHandler")
            .And().DoNotHaveName("EventBusInitialization")
            .Should()
            .NotHaveDependencyOnAny(otherModules.ToArray())
            .GetResult();

        AssertArchTestResult(result);
    }
}
