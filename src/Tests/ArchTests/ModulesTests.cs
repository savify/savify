using System.Reflection;
using App.ArchTests.SeedWork;
using App.Modules.Banks.Application.Contracts;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Infrastructure;
using App.Modules.Categories.Application.Contracts;
using App.Modules.Categories.Domain.Categories;
using App.Modules.Categories.Infrastructure;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Infrastructure;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Domain.UserNotificationSettings;
using App.Modules.Notifications.Infrastructure;
using App.Modules.Transactions.Application.Contracts;
using App.Modules.Transactions.Domain.Transactions;
using App.Modules.Transactions.Infrastructure;
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
            NotificationsNamespace,
            WalletsNamespace,
            BanksNamespace,
            CategoriesNamespace,
            TransactionsNamespace
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
            UserAccessNamespace,
            WalletsNamespace,
            BanksNamespace,
            CategoriesNamespace,
            TransactionsNamespace
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

    [Test]
    public void WalletsModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace,
            NotificationsNamespace,
            BanksNamespace,
            CategoriesNamespace,
            TransactionsNamespace
        };
        List<Assembly> walletsAssemblies = new List<Assembly>
        {
            typeof(IFinanceTrackingModule).Assembly,
            typeof(DebitWallet).Assembly,
            typeof(FinanceTrackingContext).Assembly
        };

        var result = Types.InAssemblies(walletsAssemblies)
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
    public void BanksModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace,
            NotificationsNamespace,
            WalletsNamespace,
            CategoriesNamespace,
            TransactionsNamespace
        };
        List<Assembly> banksAssemblies = new List<Assembly>
        {
            typeof(IBanksModule).Assembly,
            typeof(Bank).Assembly,
            typeof(BanksContext).Assembly
        };

        var result = Types.InAssemblies(banksAssemblies)
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
    public void CategoriesModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace,
            NotificationsNamespace,
            WalletsNamespace,
            BanksNamespace,
            TransactionsNamespace
        };
        List<Assembly> categoriesAssemblies = new List<Assembly>
        {
            typeof(ICategoriesModule).Assembly,
            typeof(Category).Assembly,
            typeof(CategoriesContext).Assembly
        };

        var result = Types.InAssemblies(categoriesAssemblies)
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
    public void TransactionsModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        var otherModules = new List<string>
        {
            UserAccessNamespace,
            NotificationsNamespace,
            WalletsNamespace,
            BanksNamespace,
            CategoriesNamespace
        };
        List<Assembly> transactionsAssemblies = new List<Assembly>
        {
            typeof(ITransactionsModule).Assembly,
            typeof(Transaction).Assembly,
            typeof(TransactionsContext).Assembly
        };

        var result = Types.InAssemblies(transactionsAssemblies)
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
